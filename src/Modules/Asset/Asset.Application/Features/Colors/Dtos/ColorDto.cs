namespace MachineryManager.Asset.Application.Features.Colors.Dtos;

/// <summary>Read-only projection of a Color for API/UI consumption.</summary>
public sealed record ColorDto(Guid Id, Guid OrganizationId, string Name);