using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Asset.Domain.Asset"/> aggregate.</summary>
public interface IAssetRepository : IRepository<global::Asset.Domain.Asset, global::Asset.Domain.AssetId>
{
    /// <summary>
    /// Performs a paginated search over Assets within the given
    /// Organization.
    /// </summary>
    Task<Features.Assets.Queries.SearchAssets.SearchAssetsResponse> SearchAsync(
        Guid organizationId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
