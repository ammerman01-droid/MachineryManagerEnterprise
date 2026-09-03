using MachineryManager.SharedKernel;

namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Engine Model.</summary>
public sealed record RegisterEngineModelRequest(
    Guid HoldingId,
    string Name,
    Guid CompanyId,
    FuelKind FuelKind,
    int? CylinderCount,
    decimal? EngineDisplacementValue,
    Guid? EngineDisplacementUnitOfMeasurementId,
    decimal? EnginePowerValue,
    Guid? EnginePowerUnitOfMeasurementId,
    decimal? WeightValue,
    Guid? WeightUnitOfMeasurementId);