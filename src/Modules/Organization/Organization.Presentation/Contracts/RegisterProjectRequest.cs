namespace MachineryManager.Organization.Presentation.Contracts;

/// <summary>
/// Request body for registering a new Project under an Organization.
/// </summary>
/// <param name="OrganizationId">The GUID of the owning organization.</param>
/// <param name="Name">The display name of the project to create.</param>
public sealed record RegisterProjectRequest(Guid OrganizationId, string Name);