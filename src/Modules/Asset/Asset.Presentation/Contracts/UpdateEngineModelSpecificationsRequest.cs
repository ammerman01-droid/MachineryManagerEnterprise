namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for updating an Engine Model's technical specifications.</summary>
/// <param name="CompanyId">The manufacturer company.</param>
/// <param name="CylinderCount">Optional number of cylinders.</param>
/// <param name="EngineDisplacementValue">Optional engine displacement value.</param>
/// <param name="EngineDisplacementUnitOfMeasurementId">Optional unit for displacement.</param>
/// <param name="EnginePowerValue">Optional engine power value.</param>
/// <param name="EnginePowerUnitOfMeasurementId">Optional unit for power.</param>
/// <param name="WeightValue">Optional weight value.</param>
/// <param name="WeightUnitOfMeasurementId">Optional unit for weight.</param>
public sealed record UpdateEngineModelSpecificationsRequest(
    Guid CompanyId,
    int? CylinderCount,
    decimal? EngineDisplacementValue,
    Guid? EngineDisplacementUnitOfMeasurementId,
    decimal? EnginePowerValue,
    Guid? EnginePowerUnitOfMeasurementId,
    decimal? WeightValue,
    Guid? WeightUnitOfMeasurementId);