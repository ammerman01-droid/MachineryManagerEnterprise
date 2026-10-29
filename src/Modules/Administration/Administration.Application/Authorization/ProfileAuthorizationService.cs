using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Authorization;

/// <inheritdoc cref="IProfileAuthorizationService" />
public sealed class ProfileAuthorizationService : IProfileAuthorizationService
{
    private const string Section = "Profile";

    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IUserAuthorizationScopeResolver _scopeResolver;
    private readonly IEffectivePermissionsProvider _effectivePermissionsProvider;
    private readonly IProfileRepository _profileRepository;

    /// <summary>Initializes a new instance of the <see cref="ProfileAuthorizationService"/> class.</summary>
    /// <param name="currentUserService">Identifies the caller.</param>
    /// <param name="permissionEvaluator">Evaluates the caller's permissions per scope.</param>
    /// <param name="scopeResolver">Resolves the caller's scope hierarchy.</param>
    /// <param name="effectivePermissionsProvider">Computes the caller's effective permissions.</param>
    /// <param name="profileRepository">Loads profiles.</param>
    public ProfileAuthorizationService(
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IUserAuthorizationScopeResolver scopeResolver,
        IEffectivePermissionsProvider effectivePermissionsProvider,
        IProfileRepository profileRepository)
    {
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _scopeResolver = scopeResolver;
        _effectivePermissionsProvider = effectivePermissionsProvider;
        _profileRepository = profileRepository;
    }

    /// <inheritdoc />
    public async Task<Result<ProfileVisibility>> ResolveVisibilityAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<ProfileVisibility>(Unauthorized());
        }

        var authorized = await _permissionEvaluator.GetAuthorizedScopesAsync(
            userId,
            PermissionCatalog.BuildPermission(Section, "View"),
            cancellationToken);

        if (authorized.IsUnrestricted)
        {
            return ProfileVisibility.Everything;
        }

        var hasAnyScope = authorized.HoldingIds.Count > 0
            || authorized.OrganizationIds.Count > 0
            || authorized.ProjectIds.Count > 0;

        if (!hasAnyScope)
        {
            return Result.Failure<ProfileVisibility>(Forbidden("view profiles"));
        }

        var scope = await _scopeResolver.ResolveAsync(userId, cancellationToken);

        // Project-level users have no part in profile administration.
        if (scope is null || scope.ProjectId is not null || scope.HoldingId is null)
        {
            return Result.Failure<ProfileVisibility>(Forbidden("view profiles"));
        }

        return new ProfileVisibility(false, scope.HoldingId);
    }

    /// <inheritdoc />
    public async Task<Result> AuthorizeManageAsync(
        string action,
        Guid? ownerHoldingId,
        IReadOnlyCollection<string>? permissionsToGrant,
        CancellationToken cancellationToken = default)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(Unauthorized());
        }

        // A resource carrying only a HoldingId is covered by Platform and by that
        // Holding's assignments, never by Organization/Project ones; a Platform-owned
        // resource (no ids) is covered by Platform assignments only.
        var scope = ownerHoldingId is { } holdingId
            ? new ResourceScope(holdingId, null, null)
            : ResourceScope.PlatformWide;

        var permission = PermissionCatalog.BuildPermission(Section, action);

        if (!await _permissionEvaluator.HasPermissionAsync(userId, permission, scope, cancellationToken))
        {
            return Result.Failure(Forbidden($"perform '{permission}' here"));
        }

        if (permissionsToGrant is { Count: > 0 })
        {
            var effective = await _effectivePermissionsProvider.GetAsync(userId, scope, cancellationToken);

            if (!effective.IncludesAll(permissionsToGrant))
            {
                return Result.Failure(Error.Failure(
                    "Profile.PermissionEscalation",
                    "A profile cannot contain permissions that the current user does not hold."));
            }
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result<Profile>> GetForManagementAsync(
        Guid profileId,
        string action,
        IReadOnlyCollection<string>? permissionsToGrant,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profileRepository.GetByIdAsync(ProfileId.From(profileId), cancellationToken);
        var visibility = await ResolveVisibilityAsync(cancellationToken);

        if (profile is null || visibility.IsFailure || !visibility.Value.CanSee(profile.HoldingId))
        {
            return Result.Failure<Profile>(ProfileErrors.ProfileNotFound(profileId));
        }

        var authorization = await AuthorizeManageAsync(action, profile.HoldingId, permissionsToGrant, cancellationToken);

        return authorization.IsFailure
            ? Result.Failure<Profile>(authorization.Error)
            : profile;
    }

    private static Error Unauthorized() => Error.Failure(
        "Profile.Unauthorized",
        "The current request is not attributable to an authenticated user.");

    private static Error Forbidden(string what) => Error.Failure(
        "Profile.Forbidden",
        $"The current user is not authorized to {what}.");
}
