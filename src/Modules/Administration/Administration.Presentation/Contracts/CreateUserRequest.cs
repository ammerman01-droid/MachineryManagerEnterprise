using Administration.Domain;

namespace MachineryManagerEnterprise.Administration.Presentation.Contracts;

/// <summary>Request body for creating a user.</summary>
/// <param name="UserName">The sign-in name.</param>
/// <param name="Password">The initial password.</param>
/// <param name="Roles">Identity roles to grant. Honoured for Platform callers only.</param>
/// <param name="ScopeLevel">
/// The level the user will work at. Omit (together with the ids and profile) to create a bare
/// Platform account that is given a profile separately — Platform callers only.
/// </param>
/// <param name="ScopeHoldingId">The Holding, when <paramref name="ScopeLevel"/> is Holding.</param>
/// <param name="ScopeOrganizationId">The Organization, when <paramref name="ScopeLevel"/> is Organization.</param>
/// <param name="ScopeProjectId">The Project, when <paramref name="ScopeLevel"/> is Project.</param>
/// <param name="ProfileId">
/// The profile to give the user. Not needed for the first Holding-level user of a Holding,
/// who always receives the system-managed full-access profile.
/// </param>
public sealed record CreateUserRequest(
    string UserName,
    string Password,
    string[]? Roles = null,
    AuthorizationScopeLevel? ScopeLevel = null,
    Guid? ScopeHoldingId = null,
    Guid? ScopeOrganizationId = null,
    Guid? ScopeProjectId = null,
    Guid? ProfileId = null);
