namespace MachineryManager.Organization.Presentation.Contracts;

/// <summary>
/// Request body for registering a new Organization (UC-1301 / CMD-950).
/// </summary>
/// <param name="Name">The display name of the organization to create.</param>
public sealed record RegisterOrganizationRequest(string Name);
