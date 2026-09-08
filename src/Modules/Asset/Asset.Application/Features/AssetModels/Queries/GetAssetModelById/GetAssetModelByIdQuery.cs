using MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Queries.GetAssetModelById;

/// <summary>Query to retrieve a single Asset Model by its identifier.</summary>
public sealed record GetAssetModelByIdQuery(Guid AssetModelId) : IRequest<Result<AssetModelDto>>;