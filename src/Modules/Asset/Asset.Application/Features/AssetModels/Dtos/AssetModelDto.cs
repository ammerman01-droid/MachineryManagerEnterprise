namespace MachineryManager.Asset.Application.Features.AssetModels.Dtos;

/// <summary>Read-only view of an Asset Model.</summary>
public sealed record AssetModelDto(
    Guid Id,
    string Name,
    Guid CompanyId,
    Guid HoldingId,
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
    Guid? WorkingCapacityWeightUnitOfMeasurementId,
    IReadOnlyCollection<Guid> CompatibleEngineModelIds);