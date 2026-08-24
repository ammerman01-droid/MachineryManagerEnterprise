namespace MachineryManager.Organization.Application.Features.Organizations.Dtos;

/// <summary>
/// Data transfer object representing a read-only view of an Organization.
/// </summary>
/// <param name="Id">The unique identifier of the organization.</param>
/// <param name="Name">The display name of the organization.</param>
/// <param name="IsSuspended">Whether the organization is currently suspended (BR-017, Section 10.16).</param>
public sealed record OrganizationDto(
    Guid Id,
    string Name,
    bool IsSuspended);