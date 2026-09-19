using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Asset.Presentation.Contracts;

/// <summary>HTTP request body for registering a new Asset.</summary>
public sealed record RegisterAssetRequest(
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
    Guid ProjectId,
    MeterReadingUnit? MeterReadingUnit,
    FuelKind? PrimaryFuelKind,
    FuelUnit? PrimaryFuelUnit,
    FuelKind? SecondaryFuelKind,
    FuelUnit? SecondaryFuelUnit);
