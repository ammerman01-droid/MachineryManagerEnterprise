using Administration.Domain;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>The caller's own position in the Holding/Organization/Project tree.</summary>
/// <param name="UserId">The caller.</param>
/// <param name="Level">The hierarchy level of the caller's active assignment.</param>
/// <param name="HoldingId">The caller's Holding (null for Platform).</param>
/// <param name="OrganizationId">The caller's Organization (Organization/Project level).</param>
/// <param name="ProjectId">The caller's Project (Project level).</param>
public sealed record ActorScope(
    Guid UserId,
    ScopeLevel Level,
    Guid? HoldingId,
    Guid? OrganizationId,
    Guid? ProjectId);

/// <summary>A scope with its full ancestor chain resolved (Project → Organization → Holding).</summary>
/// <param name="Level">The level of the scope itself.</param>
/// <param name="HoldingId">The Holding on the chain (null for Platform).</param>
/// <param name="OrganizationId">The Organization on the chain, if the scope is at or below Organization level.</param>
/// <param name="ProjectId">The Project, if the scope is at Project level.</param>
public sealed record ScopeChain(
    ScopeLevel Level,
    Guid? HoldingId,
    Guid? OrganizationId,
    Guid? ProjectId)
{
    /// <summary>Gets the Platform scope (no ancestors).</summary>
    public static ScopeChain Platform { get; } = new(ScopeLevel.Platform, null, null, null);

    /// <summary>Builds the chain of a user's home; a user without a home is a Platform user.</summary>
    /// <param name="home">The user's home, or <see langword="null"/>.</param>
    /// <returns>The home's chain.</returns>
    public static ScopeChain From(UserHome? home) =>
        home is null ? Platform : new ScopeChain(home.Level, home.HoldingId, home.OrganizationId, home.ProjectId);

    /// <summary>Converts the chain to the scope used for permission evaluation.</summary>
    /// <returns>The resource scope.</returns>
    public ResourceScope ToResourceScope() => new(HoldingId, OrganizationId, ProjectId);
}

/// <summary>Which users the caller may list.</summary>
/// <param name="All">Whether every user is visible (Platform).</param>
/// <param name="HoldingId">When not <see cref="All"/>: the Holding whose users are visible.</param>
/// <param name="OrganizationId">When not <see cref="All"/>: the Organization whose users are visible (takes precedence over the Holding).</param>
public sealed record UserVisibility(bool All, Guid? HoldingId, Guid? OrganizationId);

/// <summary>A user the caller is allowed to act on, with the user's resolved home chain.</summary>
/// <param name="Account">The user's account.</param>
/// <param name="Home">The user's home chain.</param>
public sealed record ManagedUser(UserAccount Account, ScopeChain Home);
