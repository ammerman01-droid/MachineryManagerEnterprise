using Administration.Domain;

namespace MachineryManager.Administration.Presentation.Contracts;

/// <summary>
/// Request body for assigning a User to a Profile at a specific scope.
/// </summary>
/// <param name="UserId">The GUID of the user.</param>
/// <param name="ProfileId">The GUID of the profile.</param>
/// <param name="ScopeLevel">The authorization scope level.</param>
/// <param name="ScopeHoldingId">The holding identifier for Holding-level scope.</param>
/// <param name="ScopeOrganizationId">The organization identifier for Organization-level scope.</param>
/// <param name="ScopeProjectId">The project identifier for Project-level scope.</param>
public sealed record AssignUserToProfileRequest(
    Guid UserId,
    Guid ProfileId,
    AuthorizationScopeLevel ScopeLevel,
    Guid? ScopeHoldingId,
    Guid? ScopeOrganizationId,
    Guid? ScopeProjectId);