using MachineryManager.Asset.Application.Features.AssetModels.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Queries.GetAssetModelById;

/// <summary>Query to retrieve a single Asset Model by its identifier.</summary>
public sealed record GetAssetModelByIdQuery(Guid AssetModelId) : IRequest<Result<AssetModelDto>>;