using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Asset.Application.Abstractions;

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

    /// <summary>
    /// Determines whether another Asset in the given Organization
    /// already uses the given identification code (chat, 2026-08-28 —
    /// Code is unique per Organization).
    /// </summary>
    Task<bool> ExistsWithCodeAsync(
        Guid organizationId,
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether an Asset OTHER than
    /// <paramref name="excludedAssetId"/> in the given Organization
    /// already uses the given identification code (chat, 2026-09-19 —
    /// used when an existing Asset's code is edited).
    /// </summary>
    Task<bool> ExistsOtherWithCodeAsync(
        Guid organizationId,
        string code,
        global::Asset.Domain.AssetId excludedAssetId,
        CancellationToken cancellationToken = default);
}
