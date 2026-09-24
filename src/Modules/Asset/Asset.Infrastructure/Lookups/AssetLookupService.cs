using Asset.Domain;
using MachineryManagerEnterprise.Asset.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Asset.Infrastructure.Lookups;

/// <summary>
/// EF Core implementation of <see cref="IAssetLookupService"/> —
/// exposes only the fields other modules are allowed to read
/// (chat, 2026-09-16, updated 2026-09-20 for the enum-based
/// MeterReadingUnit/PrimaryFuelUnit/SecondaryFuelUnit fields), never
/// the full <see cref="global::Asset.Domain.Asset"/> aggregate itself.
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

    /// <inheritdoc />
    public Task<bool> ExistsAsync(Guid assetId, CancellationToken cancellationToken = default) =>
        _dbContext.Assets.AsNoTracking().AnyAsync(a => a.Id == AssetId.From(assetId), cancellationToken);

    /// <inheritdoc />
    public Task<AssetConsumptionSnapshot?> GetConsumptionSnapshotAsync(
        Guid assetId, CancellationToken cancellationToken = default) =>
        _dbContext.Assets
            .AsNoTracking()
            .Where(a => a.Id == AssetId.From(assetId))
            .Select(a => new AssetConsumptionSnapshot(
                a.OrganizationId,
                a.ProjectId,
                a.MeterReadingUnit,
                a.PrimaryFuelKind,
                a.PrimaryFuelUnit,
                a.SecondaryFuelKind,
                a.SecondaryFuelUnit))
            .Cast<AssetConsumptionSnapshot?>()
            .FirstOrDefaultAsync(cancellationToken);
}
