using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Asset.Domain.AssetModel"/> aggregate.</summary>
public interface IAssetModelRepository : IRepository<global::Asset.Domain.AssetModel, global::Asset.Domain.AssetModelId>
{
    /// <summary>
    /// Performs a paginated search over asset models within the given
    /// Organization, restricted to the current user's authorized scope.
    /// </summary>
    Task<Features.AssetModels.Queries.SearchAssetModels.SearchAssetModelsResponse> SearchAsync(
        Guid organizationId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}