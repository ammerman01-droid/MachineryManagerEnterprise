namespace MachineryManager.Asset.Application.Features.Assets.Dtos;

/// <summary>Read-only projection of an Asset for API/UI consumption.</summary>
public sealed record AssetDto(
    Guid Id,
    Guid OrganizationId,
    string Code,
    Guid AssetModelId,
    string? SerialNumber,
    string? LicensePlate,
    int? ManufactureYear,
    string Color,
    string Status);