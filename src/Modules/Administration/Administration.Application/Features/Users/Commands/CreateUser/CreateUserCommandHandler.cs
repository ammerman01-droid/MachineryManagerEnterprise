using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Handles <see cref="CreateUserCommand"/>. The caller needs <c>User.Create</c> on the target scope,
/// may only create users at their own level (Holding/Platform) or below, and may only give a
/// profile whose permissions they hold themselves. The first Holding-level user of a Holding
/// receives the system-managed full-access profile, created on the spot. The account lives in
/// Identity and the home/assignment/profile in Administration, so creation is a saga: the
/// account is created first and deleted again if the Administration side fails to save.
/// </summary>
public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserAuthorizationService _authorization;
    private readonly IScopeChainResolver _chainResolver;
    private readonly IProfileRepository _profiles;
    private readonly IUserProfileAssignmentRepository _assignments;
    private readonly IUserHomeRepository _homes;
    private readonly IUserAccountService _accounts;
    private readonly IEffectivePermissionsProvider _effectivePermissions;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.</summary>
    /// <param name="authorization">Decides who may create users.</param>
    /// <param name="chainResolver">Resolves the target scope's ancestor chain.</param>
    /// <param name="profiles">The profile repository.</param>
    /// <param name="assignments">The assignment repository.</param>
    /// <param name="homes">The user-home repository.</param>
    /// <param name="accounts">Creates the Identity account.</param>
    /// <param name="effectivePermissions">Computes the caller's effective permissions.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public CreateUserCommandHandler(
        IUserAuthorizationService authorization,
        IScopeChainResolver chainResolver,
        IProfileRepository profiles,
        IUserProfileAssignmentRepository assignments,
        IUserHomeRepository homes,
        IUserAccountService accounts,
        IEffectivePermissionsProvider effectivePermissions,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _authorization = authorization;
        _chainResolver = chainResolver;
        _profiles = profiles;
        _assignments = assignments;
        _homes = homes;
        _accounts = accounts;
        _effectivePermissions = effectivePermissions;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var actorResult = await _authorization.GetActorAsync(cancellationToken);

        if (actorResult.IsFailure)
        {
            return Result.Failure<Guid>(actorResult.Error);
        }

        var actor = actorResult.Value;
        var roles = actor.Level == ScopeLevel.Platform ? request.Roles : null;

        // Bare account (no scope): Platform callers only. It has no home (= Platform user) and
        // is given its profile through the assignment endpoint afterwards.
        if (request.Scope is null)
        {
            var platformAuthorization = await _authorization.AuthorizeScopeAsync(
                actor,
                ScopeChain.Platform,
                "Create",
                cancellationToken);

            if (platformAuthorization.IsFailure)
            {
                return Result.Failure<Guid>(platformAuthorization.Error);
            }

            var bareUserId = Guid.NewGuid();
            var bareAccount = await _accounts.CreateAsync(bareUserId, request.UserName, request.Password, roles, cancellationToken);

            return bareAccount.IsFailure
                ? Result.Failure<Guid>(bareAccount.Error)
                : Result.Success(bareUserId);
        }

        var chainResult = await _chainResolver.ResolveAsync(request.Scope, cancellationToken);

        if (chainResult.IsFailure)
        {
            return Result.Failure<Guid>(chainResult.Error);
        }

        var target = chainResult.Value;

        var authorization = await _authorization.AuthorizeScopeAsync(actor, target, "Create", cancellationToken);

        if (authorization.IsFailure)
        {
            return Result.Failure<Guid>(authorization.Error);
        }

        // The first Holding-level user of a Holding gets the system full-access profile, but only
        // when the caller themself has full access there (the wildcard, or every catalog permission) (otherwise the caller must pick
        // a profile like anyone else, so a limited admin cannot mint a full-access user).
        var createSystemProfile = false;

        if (target.Level == ScopeLevel.Holding)
        {
            var existingSystem = await _profiles.GetSystemProfileByHoldingIdAsync(target.HoldingId!.Value, cancellationToken);

            if (existingSystem is null)
            {
                var effective = await _effectivePermissions.GetAsync(actor.UserId, target.ToResourceScope(), cancellationToken);
                createSystemProfile = effective.IsFullAccess;
            }
        }

        Profile profile;

        if (createSystemProfile)
        {
            var created = Profile.CreateSystemFullAccess(target.HoldingId!.Value, _dateTimeProvider);

            if (created.IsFailure)
            {
                return Result.Failure<Guid>(created.Error);
            }

            profile = created.Value;
        }
        else
        {
            if (request.ProfileId is not { } profileId)
            {
                return Result.Failure<Guid>(Error.Validation("User.ProfileRequired", "A profile must be chosen for the new user."));
            }

            var existing = await _profiles.GetByIdAsync(ProfileId.From(profileId), cancellationToken);

            if (existing is null)
            {
                return Result.Failure<Guid>(ProfileErrors.ProfileNotFound(profileId));
            }

            var profileAuthorization = await _authorization.AuthorizeProfileForAssignmentAsync(actor, target, existing, cancellationToken);

            if (profileAuthorization.IsFailure)
            {
                return Result.Failure<Guid>(profileAuthorization.Error);
            }

            profile = existing;
        }

        var userId = Guid.NewGuid();

        var account = await _accounts.CreateAsync(userId, request.UserName, request.Password, roles, cancellationToken);

        if (account.IsFailure)
        {
            return Result.Failure<Guid>(account.Error);
        }

        var assignment = UserProfileAssignment.Create(userId, profile.Id, request.Scope, _dateTimeProvider);

        if (assignment.IsFailure)
        {
            await _accounts.DeleteAsync(userId, CancellationToken.None);
            return Result.Failure<Guid>(assignment.Error);
        }

        try
        {
            if (createSystemProfile)
            {
                _profiles.Add(profile);
            }

            if (target.Level != ScopeLevel.Platform)
            {
                _homes.Add(UserHome.Create(userId, target.HoldingId, target.OrganizationId, target.ProjectId, _dateTimeProvider.UtcNow));
            }

            _assignments.Add(assignment.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            // Compensate: never leave an account that nobody can see or manage.
            await _accounts.DeleteAsync(userId, CancellationToken.None);
            throw;
        }

        return Result.Success(userId);
    }
}
