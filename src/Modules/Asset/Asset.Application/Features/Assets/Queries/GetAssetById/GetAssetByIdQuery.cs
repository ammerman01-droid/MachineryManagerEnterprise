using MachineryManager.Asset.Application.Features.Assets.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Queries.GetAssetById;

/// <summary>Query to retrieve a single Asset by its identifier.</summary>
public sealed record GetAssetByIdQuery(Guid AssetId) : IRequest<Result<AssetDto>>;
