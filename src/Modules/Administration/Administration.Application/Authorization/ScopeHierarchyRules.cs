using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Authorization;

/// <summary>The pure hierarchy rules of Section 5.8 (no I/O, fully unit-testable).</summary>
public static class ScopeHierarchyRules
{
    /// <summary>
    /// Whether a caller at <paramref name="actor"/> level may manage users at <paramref name="target"/> level:
    /// Platform and Holding may act at their own level and below; Organization only below; Project never.
    /// </summary>
    /// <param name="actor">The caller's level.</param>
    /// <param name="target">The level being managed.</param>
    /// <returns><see langword="true"/> when allowed.</returns>
    public static bool CanManageLevel(ScopeLevel actor, ScopeLevel target) => actor switch
    {
        ScopeLevel.Platform => true,
        ScopeLevel.Holding => target is ScopeLevel.Holding or ScopeLevel.Organization or ScopeLevel.Project,
        ScopeLevel.Organization => target == ScopeLevel.Project,
        _ => false,
    };

    /// <summary>Whether <paramref name="target"/> lies inside the caller's own scope subtree.</summary>
    /// <param name="actor">The caller.</param>
    /// <param name="target">The scope chain to test.</param>
    /// <returns><see langword="true"/> when contained.</returns>
    public static bool Contains(ActorScope actor, ScopeChain target) => actor.Level switch
    {
        ScopeLevel.Platform => true,
        ScopeLevel.Holding => actor.HoldingId is not null && target.HoldingId == actor.HoldingId,
        ScopeLevel.Organization => actor.OrganizationId is not null && target.OrganizationId == actor.OrganizationId,
        _ => false,
    };

    /// <summary>Whether <paramref name="target"/> lies inside a user's home node (a user can only work within it).</summary>
    /// <param name="home">The user's home chain.</param>
    /// <param name="target">The scope chain to test.</param>
    /// <returns><see langword="true"/> when inside the home.</returns>
    public static bool IsWithinHome(ScopeChain home, ScopeChain target) => home.Level switch
    {
        ScopeLevel.Platform => true,
        ScopeLevel.Holding => target.HoldingId == home.HoldingId,
        ScopeLevel.Organization => target.OrganizationId == home.OrganizationId,
        _ => target.ProjectId == home.ProjectId,
    };
}
