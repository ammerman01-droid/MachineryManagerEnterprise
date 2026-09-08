using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;

/// <summary>Command to register a new Asset Model within a Holding.</summary>
public sealed record RegisterAssetModelCommand(
    Guid HoldingId,
    string Name,
    Guid CompanyId,
    decimal? LengthValue = null,
    Guid? LengthUnitOfMeasurementId = null,
    decimal? WidthValue = null,
    Guid? WidthUnitOfMeasurementId = null,
    decimal? HeightValue = null,
    Guid? HeightUnitOfMeasurementId = null,
    decimal? WeightValue = null,
    Guid? WeightUnitOfMeasurementId = null,
    decimal? WorkingCapacityVolumeValue = null,
    Guid? WorkingCapacityVolumeUnitOfMeasurementId = null,
    decimal? WorkingCapacityWeightValue = null,
    Guid? WorkingCapacityWeightUnitOfMeasurementId = null)
    : IRequest<Result<Guid>>;