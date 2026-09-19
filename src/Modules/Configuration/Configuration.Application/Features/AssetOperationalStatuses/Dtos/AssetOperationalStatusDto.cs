namespace MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Dtos;

/// <summary>Read-only projection of an AssetOperationalStatus for API/UI consumption.</summary>
public sealed record AssetOperationalStatusDto(Guid Id, string Name);