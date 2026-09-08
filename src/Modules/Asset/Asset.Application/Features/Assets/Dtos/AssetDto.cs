namespace MachineryManager.Asset.Application.Features.Assets.Dtos;

/// <summary>Read-only projection of an Asset for API/UI consumption.</summary>
public sealed record AssetDto(
    Guid Id,
    Guid OrganizationId,
    string Code,
    string Name,
    Guid AssetModelId,
    Guid ColorId,
    string? SerialNumber,
    string? ChassisNumber,
    string? BodyNumber,
    string? Vin,
    string? LicensePlate,
    int? ManufactureYear,
    string Status);