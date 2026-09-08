namespace MachineryManagerEnterprise.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Asset Model.</summary>
public sealed record RegisterAssetModelRequest(
    Guid HoldingId,
    string Name,
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