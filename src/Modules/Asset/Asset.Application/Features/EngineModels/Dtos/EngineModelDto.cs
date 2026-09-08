using MachineryManager.SharedKernel;

namespace MachineryManager.Asset.Application.Features.EngineModels.Dtos;

/// <summary>Read-only view of an Engine Model.</summary>
public sealed record EngineModelDto(
    Guid Id,
    string Name,
    Guid CompanyId,
    FuelKind FuelKind,
    int? CylinderCount,
    decimal? EngineDisplacementValue,
    Guid? EngineDisplacementUnitOfMeasurementId,
    decimal? EnginePowerValue,
    Guid? EnginePowerUnitOfMeasurementId,
    decimal? WeightValue,
    Guid? WeightUnitOfMeasurementId,
    Guid HoldingId);