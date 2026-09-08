namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for updating an existing Asset Model's technical specifications.</summary>
public sealed record UpdateAssetModelSpecificationsRequest(
    Guid CompanyId,
    decimal? LengthValue,
    Guid? LengthUnitOfMeasurementId,
    decimal? WidthValue,
    Guid? WidthUnitOfMeasurementId,
    decimal? HeightValue,
    Guid? HeightUnitOfMeasurementId,
    decimal? WeightValue,
    Guid? WeightUnitOfMeasurementId,
    decimal? WorkingCapacityVolumeValue,
    Guid? WorkingCapacityVolumeUnitOfMeasurementId,
    decimal? WorkingCapacityWeightValue,
    Guid? WorkingCapacityWeightUnitOfMeasurementId);