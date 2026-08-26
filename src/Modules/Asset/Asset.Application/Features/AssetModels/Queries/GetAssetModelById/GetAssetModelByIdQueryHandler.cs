using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.AssetModels.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Queries.GetAssetModelById;

/// <summary>Handles <see cref="GetAssetModelByIdQuery"/> by loading the aggregate and mapping it to a DTO.</summary>
public sealed class GetAssetModelByIdQueryHandler
    : IRequestHandler<GetAssetModelByIdQuery, Result<AssetModelDto>>
{
    private readonly IAssetModelRepository _assetModelRepository;

    /// <summary>Initializes a new instance of the <see cref="GetAssetModelByIdQueryHandler"/> class.</summary>
    public GetAssetModelByIdQueryHandler(IAssetModelRepository assetModelRepository)
    {
        _assetModelRepository = assetModelRepository;
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

        var dto = new AssetModelDto(
            assetModel.Id.Value,
            assetModel.Name,
            assetModel.Manufacturer,
            assetModel.CompatibleEngineModelIds.Select(x => x.Value).ToList());

        return Result.Success(dto);
    }
}