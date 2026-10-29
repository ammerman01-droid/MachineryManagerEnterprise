using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.Administration.Application.Authorization;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.UserProfileAssignments.Commands.AssignUserToProfile;

/// <summary>
/// Handles <see cref="AssignUserToProfileCommand"/>. The caller must be allowed to manage the
/// user (level, scope, <c>User.Edit</c>), the target scope must lie inside both the caller's
/// scope and the user's home, and the profile must belong to the target's Holding and contain
/// only permissions the caller holds. The user's previously active assignment is deactivated,
/// and all of the user's sessions end so the new rights apply at once.
/// </summary>
public sealed class AssignUserToProfileCommandHandler
    : IRequestHandler<AssignUserToProfileCommand, Result<Guid>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserAuthorizationService _authorization;
    private readonly IScopeChainResolver _chainResolver;
    private readonly IUserSessionInvalidator _sessionInvalidator;

    /// <summary>Initializes a new instance of the <see cref="AssignUserToProfileCommandHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="authorization">Decides who may manage users and assignments.</param>
    /// <param name="chainResolver">Resolves the target scope's ancestor chain.</param>
    /// <param name="sessionInvalidator">Ends the user's sessions so the new assignment takes effect.</param>
    public AssignUserToProfileCommandHandler(
        IProfileRepository profileRepository,
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IUserAuthorizationService authorization,
        IScopeChainResolver chainResolver,
        IUserSessionInvalidator sessionInvalidator)
    {
        _profileRepository = profileRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _authorization = authorization;
        _chainResolver = chainResolver;
        _sessionInvalidator = sessionInvalidator;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(
        AssignUserToProfileCommand request,
        CancellationToken cancellationToken)
    {
        var actorResult = await _authorization.GetActorAsync(cancellationToken);

        if (actorResult.IsFailure)
        {
            return Result.Failure<Guid>(actorResult.Error);
        }

        var actor = actorResult.Value;

        var user = await _authorization.LoadUserForManagementAsync(actor, request.UserId, "Edit", cancellationToken);

        if (user.IsFailure)
        {
            return Result.Failure<Guid>(user.Error);
        }

        var chainResult = await _chainResolver.ResolveAsync(request.Scope, cancellationToken);

        if (chainResult.IsFailure)
        {
            return Result.Failure<Guid>(chainResult.Error);
        }

        var target = chainResult.Value;

        var scopeAuthorization = await _authorization.AuthorizeScopeAsync(actor, target, "Edit", cancellationToken);

        if (scopeAuthorization.IsFailure)
        {
            return Result.Failure<Guid>(scopeAuthorization.Error);
        }

        if (!ScopeHierarchyRules.IsWithinHome(user.Value.Home, target))
        {
            return Result.Failure<Guid>(Error.Failure(
                "User.Forbidden",
                "The target scope lies outside the node this user belongs to."));
        }

        var profileId = ProfileId.From(request.ProfileId);
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            return Result.Failure<Guid>(ProfileErrors.ProfileNotFound(request.ProfileId));
        }

        var profileAuthorization = await _authorization.AuthorizeProfileForAssignmentAsync(actor, target, profile, cancellationToken);

        if (profileAuthorization.IsFailure)
        {
            return Result.Failure<Guid>(profileAuthorization.Error);
        }

        // Automatically deactivate whichever assignment currently holds
        // this user's active slot (chat, 2026-08-25 — revised). This
        // replaces the previous hard 409 conflict: assigning a new
        // Profile now always succeeds and simply takes over the active
        // slot, while the previous assignment stays in the user's list
        // (inactive) and can be reactivated later.
        var currentlyActive = await _assignmentRepository.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken);

        foreach (var existing in currentlyActive)
        {
            existing.Deactivate(_dateTimeProvider);
            _assignmentRepository.Update(existing);
        }

        var result = UserProfileAssignment.Create(
            request.UserId,
            profileId,
            request.Scope,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _assignmentRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _sessionInvalidator.InvalidateAsync(request.UserId, cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
