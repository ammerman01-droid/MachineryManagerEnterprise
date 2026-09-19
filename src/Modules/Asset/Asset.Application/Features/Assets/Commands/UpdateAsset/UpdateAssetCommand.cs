using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.UpdateAsset;

/// <summary>Command to update an existing Asset's mutable details (chat, 2026-09-16).</summary>
public sealed record UpdateAssetCommand(
    Guid AssetId,
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
    FuelUnit? SecondaryFuelUnit) : IRequest<Result>;
