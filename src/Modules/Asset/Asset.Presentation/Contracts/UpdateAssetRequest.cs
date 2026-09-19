using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Asset.Presentation.Contracts;

/// <summary>HTTP request body for updating an existing Asset's mutable details.</summary>
public sealed record UpdateAssetRequest(
    string Code,
    string Name,
    Guid AssetModelId,
    Guid ColorId,
    Guid ProjectId,
    string? SerialNumber,
    string? ChassisNumber,
    string? BodyNumber,
    string? Vin,
    string? LicensePlate,
    int? ManufactureYear,
    MeterReadingUnit? MeterReadingUnit,
    FuelKind? PrimaryFuelKind,
    FuelUnit? PrimaryFuelUnit,
    FuelKind? SecondaryFuelKind,
    FuelUnit? SecondaryFuelUnit);
