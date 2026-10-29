using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.UserProfileAssignments.Commands.DeleteUserProfileAssignment;

/// <summary>
/// Handles <see cref="DeleteUserProfileAssignmentCommand"/>: the caller must be allowed to manage the user and
/// the assignment's scope (level, containment, <c>User.Edit</c>). All of the user's sessions
/// end afterwards so the change takes effect at once.
/// </summary>
public sealed class DeleteUserProfileAssignmentCommandHandler
    : IRequestHandler<DeleteUserProfileAssignmentCommand, Result>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserAuthorizationService _authorization;
    private readonly IScopeChainResolver _chainResolver;
    private readonly IUserSessionInvalidator _sessionInvalidator;

    /// <summary>Initializes a new instance of the <see cref="DeleteUserProfileAssignmentCommandHandler"/> class.</summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="authorization">Decides who may manage users and assignments.</param>
    /// <param name="chainResolver">Resolves the assignment scope's ancestor chain.</param>
    /// <param name="sessionInvalidator">Ends the user's sessions so the change takes effect.</param>
    public DeleteUserProfileAssignmentCommandHandler(
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IUserAuthorizationService authorization,
        IScopeChainResolver chainResolver,
        IUserSessionInvalidator sessionInvalidator)
    {
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _authorization = authorization;
        _chainResolver = chainResolver;
        _sessionInvalidator = sessionInvalidator;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(
        DeleteUserProfileAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignmentId = UserProfileAssignmentId.From(request.AssignmentId);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(ProfileErrors.AssignmentNotFound(request.AssignmentId));
        }

        var actorResult = await _authorization.GetActorAsync(cancellationToken);

        if (actorResult.IsFailure)
        {
            return Result.Failure(actorResult.Error);
        }

        var actor = actorResult.Value;

        var user = await _authorization.LoadUserForManagementAsync(actor, assignment.UserId, "Edit", cancellationToken);

        if (user.IsFailure)
        {
            return Result.Failure(user.Error);
        }

        if (actor.Level != ScopeLevel.Platform)
        {
            var chain = await _chainResolver.ResolveAsync(assignment.Scope, cancellationToken);

            if (chain.IsFailure)
            {
                return Result.Failure(chain.Error);
            }

            var scopeAuthorization = await _authorization.AuthorizeScopeAsync(actor, chain.Value, "Edit", cancellationToken);

            if (scopeAuthorization.IsFailure)
            {
                return scopeAuthorization;
            }
        }

        _assignmentRepository.Remove(assignment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _sessionInvalidator.InvalidateAsync(assignment.UserId, cancellationToken);

        return Result.Success();
    }
}
