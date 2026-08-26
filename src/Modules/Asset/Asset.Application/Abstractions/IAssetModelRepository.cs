using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Asset.Domain.AssetModel"/> aggregate.</summary>
public interface IAssetModelRepository : IRepository<global::Asset.Domain.AssetModel, global::Asset.Domain.AssetModelId>
{
    /// <summary>
    /// Performs a paginated search over asset models within the given
    /// Holding.
    /// </summary>
    Task<Features.AssetModels.Queries.SearchAssetModels.SearchAssetModelsResponse> SearchAsync(
        Guid holdingId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}