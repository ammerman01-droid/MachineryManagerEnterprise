using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Queries.SearchAssetModels;

/// <summary>Handles <see cref="SearchAssetModelsQuery"/> by delegating to the repository search projection.</summary>
public sealed class SearchAssetModelsQueryHandler
    : IRequestHandler<SearchAssetModelsQuery, Result<SearchAssetModelsResponse>>
{
    private readonly IAssetModelRepository _assetModelRepository;

    /// <summary>Initializes a new instance of the <see cref="SearchAssetModelsQueryHandler"/> class.</summary>
    public SearchAssetModelsQueryHandler(IAssetModelRepository assetModelRepository)
    {
        _assetModelRepository = assetModelRepository;
    }

    /// <summary>Executes the search query.</summary>
    public async Task<Result<SearchAssetModelsResponse>> Handle(
        SearchAssetModelsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _assetModelRepository.SearchAsync(
            request.OrganizationId,
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}