using MachineryManagerEnterprise.Asset.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Asset.Domain;

namespace MachineryManagerEnterprise.Asset.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IAssetLookupService"/>,
/// providing other modules (currently Consumption) with read-only,
/// cross-module access to Asset data without depending on
/// Asset.Domain/Asset.Application directly (Modular Monolith boundary —
/// same pattern as MachineryManagerEnterprise.Organization.Infrastructure.OrganizationLookupService/>).
/// </summary>
public sealed class AssetLookupService : IAssetLookupService
{
    private readonly AssetDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="AssetLookupService"/> class.</summary>
    /// <param name="dbContext">The Asset module's persistence context.</param>
    public AssetLookupService(AssetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>Determines whether an Asset with the given identifier exists.</summary>
    /// <param name="assetId">The identifier of the Asset to look up.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><see langword="true"/> if the Asset exists; otherwise <see langword="false"/>.</returns>
    public async Task<bool> ExistsAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var id = AssetId.From(assetId);

        return await _dbContext.Assets
            .AsNoTracking()
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves the subset of an Asset's fields needed to record fuel
    /// consumption against it.
    /// </summary>
    /// <param name="assetId">The identifier of the Asset to look up.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    /// The Asset's consumption-relevant fields, or <see langword="null"/>
    /// if the Asset does not exist.
    /// </returns>
    public async Task<AssetConsumptionSnapshot?> GetConsumptionSnapshotAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var id = AssetId.From(assetId);

        return await _dbContext.Assets
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AssetConsumptionSnapshot(
                a.OrganizationId,
                a.ProjectId,
                a.MeterReadingUnit,
                a.PrimaryFuelKind,
                a.PrimaryFuelUnit,
                a.SecondaryFuelKind,
                a.SecondaryFuelUnit))
            .FirstOrDefaultAsync(cancellationToken);
    }
}