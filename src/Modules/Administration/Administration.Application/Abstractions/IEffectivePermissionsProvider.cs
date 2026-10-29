using Administration.Domain;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>
/// Computes the full set of permissions a user effectively holds within a given scope,
/// in a single round trip. Used for the anti-escalation rule: nobody may grant a
/// permission they do not hold themselves.
/// </summary>
public interface IEffectivePermissionsProvider
{
    /// <summary>
    /// Returns the union of permissions of every active assignment (with an active profile)
    /// of <paramref name="userId"/> whose scope covers <paramref name="scope"/>.
    /// </summary>
    /// <param name="userId">The user whose permissions are computed.</param>
    /// <param name="scope">The scope the permissions must apply to.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The effective permissions (possibly <see cref="EffectivePermissions.None"/>).</returns>
    Task<EffectivePermissions> GetAsync(
        Guid userId,
        ResourceScope scope,
        CancellationToken cancellationToken = default);
}

/// <summary>The permissions a user effectively holds within one scope.</summary>
public sealed class EffectivePermissions
{
    /// <summary>Gets an instance that holds no permission at all.</summary>
    public static EffectivePermissions None { get; } =
        new(false, new HashSet<string>(StringComparer.Ordinal));

    /// <summary>Initializes a new instance of the <see cref="EffectivePermissions"/> class.</summary>
    /// <param name="hasWildcard">Whether a covering profile grants every permission ("*").</param>
    /// <param name="permissions">The explicitly granted permission strings.</param>
    public EffectivePermissions(bool hasWildcard, IReadOnlySet<string> permissions)
    {
        HasWildcard = hasWildcard;
        Permissions = permissions;
    }

    /// <summary>Gets whether a covering profile grants the wildcard ("*") permission.</summary>
    public bool HasWildcard { get; }

    /// <summary>Gets the explicitly granted permission strings.</summary>
    public IReadOnlySet<string> Permissions { get; }

    /// <summary>
    /// Gets whether the user effectively has full access within the scope: either the wildcard
    /// or every permission of the <see cref="PermissionCatalog"/> granted explicitly (the way a
    /// profile built by ticking every box in the permission matrix looks). Only full-access users
    /// may create the system full-access profile of a Holding or give out a profile containing "*".
    /// </summary>
    public bool IsFullAccess => HasWildcard || AllCatalogPermissions.All(Permissions.Contains);

    private static IReadOnlyList<string> AllCatalogPermissions { get; } =
        PermissionCatalog.Sections
            .SelectMany(section => section.EffectiveActions
                .Select(action => PermissionCatalog.BuildPermission(section.Key, action)))
            .ToList();

    /// <summary>Returns whether the user holds <paramref name="permission"/>.</summary>
    /// <param name="permission">The permission string (e.g. "Profile.View", or "*" for full access).</param>
    /// <returns><see langword="true"/> when held.</returns>
    public bool Includes(string permission) =>
        permission == Profile.WildcardPermission
            ? IsFullAccess
            : HasWildcard || Permissions.Contains(permission);

    /// <summary>Returns whether the user holds every permission in <paramref name="permissions"/>.</summary>
    /// <param name="permissions">The permissions that would be granted to someone else.</param>
    /// <returns><see langword="true"/> when all are held.</returns>
    public bool IncludesAll(IEnumerable<string> permissions) => permissions.All(Includes);
}
