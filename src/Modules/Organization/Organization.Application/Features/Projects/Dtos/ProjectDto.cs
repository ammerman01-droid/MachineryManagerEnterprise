namespace MachineryManager.Organization.Application.Features.Projects.Dtos;

/// <summary>Read-only view of a Project.</summary>
public sealed record ProjectDto(
    Guid Id,
    string Name,
    Guid OrganizationId);