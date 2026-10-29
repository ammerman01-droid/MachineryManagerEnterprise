namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Adds the caller's own position in the tenant tree to an <see cref="AuthorizedScopeSet"/> for
/// read-only context pickers. Permissions grant visibility top-down only, so a user working at
/// Organization or Project level would otherwise never see the Holding (or Organization) they
/// belong to, and every page that starts with "choose a Holding" would stay empty for them.
/// Only the user's OWN node is added — never siblings or anything below it.
/// </summary>
public static class AuthorizedScopeSetExtensions
{
    /// <summary>Includes the caller's own Holding in the visible Holdings.</summary>
    /// <param name="scope">The scope granted by permissions.</param>
    /// <param name="own">The caller's resolved position, or <see langword="null"/>.</param>
    /// <returns>The extended scope (unchanged for unrestricted callers or when no Holding is known).</returns>
    public static AuthorizedScopeSet IncludingOwnHolding(this AuthorizedScopeSet scope, ResolvedUserScope? own) =>
        scope.IsUnrestricted || own?.HoldingId is not { } holdingId
            ? scope
            : scope with { HoldingIds = scope.HoldingIds.Append(holdingId).Distinct().ToList() };

    /// <summary>Includes the caller's own Organization in the visible Organizations.</summary>
    /// <param name="scope">The scope granted by permissions.</param>
    /// <param name="own">The caller's resolved position, or <see langword="null"/>.</param>
    /// <returns>The extended scope (unchanged for unrestricted callers or when no Organization is known).</returns>
    public static AuthorizedScopeSet IncludingOwnOrganization(this AuthorizedScopeSet scope, ResolvedUserScope? own) =>
        scope.IsUnrestricted || own?.OrganizationId is not { } organizationId
            ? scope
            : scope with { OrganizationIds = scope.OrganizationIds.Append(organizationId).Distinct().ToList() };

    /// <summary>Includes the caller's own Project in the visible Projects.</summary>
    /// <param name="scope">The scope granted by permissions.</param>
    /// <param name="own">The caller's resolved position, or <see langword="null"/>.</param>
    /// <returns>The extended scope (unchanged for unrestricted callers or when no Project is known).</returns>
    public static AuthorizedScopeSet IncludingOwnProject(this AuthorizedScopeSet scope, ResolvedUserScope? own) =>
        scope.IsUnrestricted || own?.ProjectId is not { } projectId
            ? scope
            : scope with { ProjectIds = scope.ProjectIds.Append(projectId).Distinct().ToList() };
}
