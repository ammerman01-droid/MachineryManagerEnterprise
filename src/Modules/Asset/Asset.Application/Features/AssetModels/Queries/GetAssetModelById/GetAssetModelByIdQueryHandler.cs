using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.AssetModels.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Queries.GetAssetModelById;

/// <summary>
/// Handles <see cref="GetAssetModelByIdQuery"/> by loading the
/// aggregate, verifying the caller is authorized for its Holding, and
/// mapping it to a DTO.
/// </summary>
public sealed class GetAssetModelByIdQueryHandler
    : IRequestHandler<GetAssetModelByIdQuery, Result<AssetModelDto>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetAssetModelByIdQueryHandler"/> class.</summary>
    public GetAssetModelByIdQueryHandler(
        IAssetModelRepository assetModelRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the lookup use case.</summary>
    public async Task<Result<AssetModelDto>> Handle(GetAssetModelByIdQuery request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.AssetModelId.From(request.AssetModelId);
        var assetModel = await _assetModelRepository.GetByIdAsync(id, cancellationToken);

        if (assetModel is null)
        {
            return Result.Failure<AssetModelDto>(
                Error.NotFound("AssetModel.NotFound", $"Asset model with id {request.AssetModelId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<AssetModelDto>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(assetModel.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<AssetModelDto>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var dto = new AssetModelDto(
            assetModel.Id.Value,
            assetModel.Name,
            assetModel.CompanyId,
            assetModel.HoldingId,
            assetModel.LengthValue,
            assetModel.LengthUnitOfMeasurementId,
            assetModel.WidthValue,
            assetModel.WidthUnitOfMeasurementId,
            assetModel.HeightValue,
            assetModel.HeightUnitOfMeasurementId,
            assetModel.WeightValue,
            assetModel.WeightUnitOfMeasurementId,
            assetModel.WorkingCapacityVolumeValue,
            assetModel.WorkingCapacityVolumeUnitOfMeasurementId,
            assetModel.WorkingCapacityWeightValue,
            assetModel.WorkingCapacityWeightUnitOfMeasurementId,
            assetModel.CompatibleEngineModelIds.Select(x => x.Value).ToList());

        return Result.Success(dto);
    }
}