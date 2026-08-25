namespace MachineryManager.Organization.Application.Features.Organizations.Dtos;

/// <summary>
/// Data transfer object representing a read-only view of an Organization.
/// </summary>
/// <param name="Id">The unique identifier of the organization.</param>
/// <param name="Name">The display name of the organization.</param>
/// <param name="IsSuspended">Whether the organization is currently suspended (BR-017, Section 10.16).</param>
/// <param name="HoldingId">The identifier of the Holding this organization is currently assigned to, or <see langword="null"/> if unassigned.</param>
public sealed record OrganizationDto(
    Guid Id,
    string Name,
    bool IsSuspended,
    Guid? HoldingId);