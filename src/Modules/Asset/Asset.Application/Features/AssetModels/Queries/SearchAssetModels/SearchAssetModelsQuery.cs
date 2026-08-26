using MachineryManager.Asset.Application.Features.AssetModels.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Queries.SearchAssetModels;

/// <summary>Query to search Asset Models within an Organization.</summary>
public sealed record SearchAssetModelsQuery(Guid OrganizationId, string? SearchTerm, int Page = 1, int PageSize = 20)
    : IRequest<Result<SearchAssetModelsResponse>>;

/// <summary>Paginated response for <see cref="SearchAssetModelsQuery"/>.</summary>
public sealed record SearchAssetModelsResponse(
    IReadOnlyCollection<AssetModelDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);