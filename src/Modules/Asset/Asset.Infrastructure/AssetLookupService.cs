using MachineryManagerEnterprise.Asset.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Asset.Domain;

namespace MachineryManagerEnterprise.Asset.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IAssetLookupService"/>,
/// providing other modules (Consumption, Maintenance) with read-only,
/// cross-module access to Asset data without depending on
/// Asset.Domain/Asset.Application directly (Modular Monolith boundary —
/// same pattern as MachineryManagerEnterprise.Organization.Infrastructure.OrganizationLookupService).
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
    public async Task<bool> ExistsAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var id = AssetId.From(assetId);

        return await _dbContext.Assets
            .AsNoTracking()
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<Guid?> GetOrganizationIdAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var id = AssetId.From(assetId);

        return await _dbContext.Assets
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => (Guid?)a.OrganizationId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    /// <remarks>
    /// The single Project-lookup method (chat, 2026-09-22) — Maintenance's
    /// Work Order registration/edit now calls this instead of a separate
    /// "GetProjectIdAsync", which has been removed to avoid two methods
    /// answering the same question.
    /// </remarks>
    public async Task<Guid?> GetCurrentProjectIdAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var id = AssetId.From(assetId);

        return await _dbContext.Assets
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => (Guid?)a.ProjectId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string?> GetCodeAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var id = AssetId.From(assetId);
        var asset = await _dbContext.Assets.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        return asset?.Code;
    }

    /// <inheritdoc />
    public async Task<string?> GetNameAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var id = AssetId.From(assetId);
        var asset = await _dbContext.Assets.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        return asset?.Name;
    }
}