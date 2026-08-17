namespace MachineryManager.Organization.Application.Features.Organizations.Dtos;

/// <summary>
/// Data transfer object representing a read-only view of an Organization.
/// </summary>
/// <param name="Id">The unique identifier of the organization.</param>
/// <param name="Name">The display name of the organization.</param>
public sealed record OrganizationDto(
    Guid Id,
    string Name);