using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.UpdateAssetModelSpecifications;

/// <summary>Command to update the technical specifications of an existing Asset Model.</summary>
public sealed record UpdateAssetModelSpecificationsCommand(
    Guid AssetModelId,
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
    Guid? WorkingCapacityWeightUnitOfMeasurementId)
    : IRequest<Result>;