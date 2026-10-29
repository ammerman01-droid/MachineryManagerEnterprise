using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Authorization;

/// <inheritdoc cref="IUserAuthorizationService" />
public sealed class UserAuthorizationService : IUserAuthorizationService
{
    private const string Section = "User";

    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IUserAuthorizationScopeResolver _scopeResolver;
    private readonly IEffectivePermissionsProvider _effectivePermissionsProvider;
    private readonly IUserAccountService _accounts;
    private readonly IUserHomeRepository _homes;

    /// <summary>Initializes a new instance of the <see cref="UserAuthorizationService"/> class.</summary>
    /// <param name="currentUserService">Identifies the caller.</param>
    /// <param name="permissionEvaluator">Evaluates the caller's permissions per scope.</param>
    /// <param name="scopeResolver">Resolves the caller's scope hierarchy.</param>
    /// <param name="effectivePermissionsProvider">Computes the caller's effective permissions.</param>
    /// <param name="accounts">Loads user accounts.</param>
    /// <param name="homes">Loads user homes.</param>
    public UserAuthorizationService(
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IUserAuthorizationScopeResolver scopeResolver,
        IEffectivePermissionsProvider effectivePermissionsProvider,
        IUserAccountService accounts,
        IUserHomeRepository homes)
    {
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _scopeResolver = scopeResolver;
        _effectivePermissionsProvider = effectivePermissionsProvider;
        _accounts = accounts;
        _homes = homes;
    }

    /// <inheritdoc />
    public async Task<Result<ActorScope>> GetActorAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<ActorScope>(Error.Failure(
                "User.Unauthorized",
                "The current request is not attributable to an authenticated user."));
        }

        var scope = await _scopeResolver.ResolveAsync(userId, cancellationToken);

        if (scope is null)
        {
            return Result.Failure<ActorScope>(Forbidden("The current user has no active profile assignment."));
        }

        var level = scope.ProjectId is not null ? ScopeLevel.Project
            : scope.OrganizationId is not null ? ScopeLevel.Organization
            : scope.HoldingId is not null ? ScopeLevel.Holding
            : ScopeLevel.Platform;

        return new ActorScope(userId, level, scope.HoldingId, scope.OrganizationId, scope.ProjectId);
    }

    /// <inheritdoc />
    public async Task<Result> AuthorizeScopeAsync(
        ActorScope actor,
        ScopeChain target,
        string action,
        CancellationToken cancellationToken = default)
    {
        if (!ScopeHierarchyRules.CanManageLevel(actor.Level, target.Level))
        {
            return Result.Failure(Forbidden("Users of your level cannot manage users or assignments at the requested level."));
        }

        if (!ScopeHierarchyRules.Contains(actor, target))
        {
            return Result.Failure(Forbidden("The requested scope is outside your own scope."));
        }

        return await CheckPermissionAsync(actor, action, target, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Result<ManagedUser>> LoadUserForManagementAsync(
        ActorScope actor,
        Guid userId,
        string action,
        CancellationToken cancellationToken = default)
    {
        var loaded = await LoadVisibleAsync(actor, userId, cancellationToken);

        if (loaded.IsFailure)
        {
            return loaded;
        }

        if (!ScopeHierarchyRules.CanManageLevel(actor.Level, loaded.Value.Home.Level))
        {
            return Result.Failure<ManagedUser>(Forbidden("Users of your level cannot manage this user."));
        }

        var permission = await CheckPermissionAsync(actor, action, loaded.Value.Home, cancellationToken);

        return permission.IsFailure ? Result.Failure<ManagedUser>(permission.Error) : loaded;
    }

    /// <inheritdoc />
    public async Task<Result<ManagedUser>> LoadUserForViewAsync(
        ActorScope actor,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var loaded = await LoadVisibleAsync(actor, userId, cancellationToken);

        if (loaded.IsFailure)
        {
            return loaded;
        }

        var permission = await CheckPermissionAsync(actor, "View", loaded.Value.Home, cancellationToken);

        return permission.IsFailure ? Result.Failure<ManagedUser>(permission.Error) : loaded;
    }

    /// <inheritdoc />
    public async Task<Result<UserVisibility>> ResolveUserVisibilityAsync(CancellationToken cancellationToken = default)
    {
        var actorResult = await GetActorAsync(cancellationToken);

        if (actorResult.IsFailure)
        {
            return Result.Failure<UserVisibility>(actorResult.Error);
        }

        var actor = actorResult.Value;

        var authorized = await _permissionEvaluator.GetAuthorizedScopesAsync(
            actor.UserId,
            PermissionCatalog.BuildPermission(Section, "View"),
            cancellationToken);

        if (authorized.IsUnrestricted)
        {
            return new UserVisibility(true, null, null);
        }

        return actor.Level switch
        {
            ScopeLevel.Holding when actor.HoldingId is not null && HasAnyScope(authorized) =>
                new UserVisibility(false, actor.HoldingId, null),
            ScopeLevel.Organization when actor.OrganizationId is not null && HasAnyScope(authorized) =>
                new UserVisibility(false, null, actor.OrganizationId),
            _ => Result.Failure<UserVisibility>(Forbidden("The current user is not authorized to view users.")),
        };
    }

    /// <inheritdoc />
    public async Task<Result> AuthorizeProfileForAssignmentAsync(
        ActorScope actor,
        ScopeChain target,
        Profile profile,
        CancellationToken cancellationToken = default)
    {
        if (!profile.IsActive)
        {
            return Result.Failure(Error.Conflict("Profile.Inactive", "Cannot assign an inactive profile to a user."));
        }

        if (profile.HoldingId != target.HoldingId)
        {
            return Result.Failure(Error.Validation(
                "Profile.OwnerMismatch",
                "The profile belongs to a different Holding than the target scope."));
        }

        var effective = await _effectivePermissionsProvider.GetAsync(actor.UserId, target.ToResourceScope(), cancellationToken);

        return effective.IncludesAll(profile.Permissions)
            ? Result.Success()
            : Result.Failure(Error.Failure(
                "Profile.PermissionEscalation",
                "The profile contains permissions that the current user does not hold."));
    }

    private async Task<Result<ManagedUser>> LoadVisibleAsync(
        ActorScope actor,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var account = await _accounts.GetAsync(userId, cancellationToken);
        var home = ScopeChain.From(await _homes.GetAsync(userId, cancellationToken));

        // A user outside the caller's scope is reported as missing, so other tenants' users
        // (and Platform users) are never revealed.
        if (account is null || !ScopeHierarchyRules.Contains(actor, home))
        {
            return Result.Failure<ManagedUser>(Error.NotFound("User.NotFound", $"User with id {userId} was not found."));
        }

        return new ManagedUser(account, home);
    }

    private async Task<Result> CheckPermissionAsync(
        ActorScope actor,
        string action,
        ScopeChain scope,
        CancellationToken cancellationToken)
    {
        var permission = PermissionCatalog.BuildPermission(Section, action);

        return await _permissionEvaluator.HasPermissionAsync(actor.UserId, permission, scope.ToResourceScope(), cancellationToken)
            ? Result.Success()
            : Result.Failure(Forbidden($"The current user is not authorized to perform '{permission}' here."));
    }

    private static bool HasAnyScope(AuthorizedScopeSet set) =>
        set.HoldingIds.Count > 0 || set.OrganizationIds.Count > 0 || set.ProjectIds.Count > 0;

    private static Error Forbidden(string message) => Error.Failure("User.Forbidden", message);
}
