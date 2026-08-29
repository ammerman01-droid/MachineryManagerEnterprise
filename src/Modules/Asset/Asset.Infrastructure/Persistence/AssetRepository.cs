using Asset.Domain;
using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.Assets.Dtos;
using MachineryManager.Asset.Application.Features.Assets.Queries.SearchAssets;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Asset.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IAssetRepository"/>.</summary>
public sealed class AssetRepository : IAssetRepository
{
    private readonly AssetDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="AssetRepository"/> class.</summary>
    /// <param name="dbContext">The Asset module's persistence context.</param>
    public AssetRepository(AssetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<global::Asset.Domain.Asset?> GetByIdAsync(AssetId id, CancellationToken cancellationToken = default) =>
        _dbContext.Assets.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(global::Asset.Domain.Asset aggregate) => _dbContext.Assets.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::Asset.Domain.Asset aggregate) => _dbContext.Assets.Update(aggregate);

    /// <inheritdoc />
    public void Remove(global::Asset.Domain.Asset aggregate) => _dbContext.Assets.Remove(aggregate);

    /// <inheritdoc />
    public Task<bool> ExistsWithCodeAsync(Guid organizationId, string code, CancellationToken cancellationToken = default) =>
        _dbContext.Assets.AnyAsync(
            a => a.OrganizationId == organizationId && a.Code == code,
            cancellationToken);

    /// <inheritdoc />
    public async Task<SearchAssetsResponse> SearchAsync(
        Guid organizationId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Assets
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(a =>
                a.Code.Contains(searchTerm) ||
                (a.SerialNumber != null && a.SerialNumber.Contains(searchTerm)) ||
                (a.LicensePlate != null && a.LicensePlate.Contains(searchTerm)));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        // Materialize entities first, map to DTO in memory — avoids the
        // EF Core 10 Select-projection translation issue hit earlier
        // with Profile.Permissions / AssetModel.CompatibleEngineModelIds
        // (chat, 2026-08-25).
        var entities = await query
            .OrderBy(a => a.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = entities
            .Select(a => new AssetDto(
                a.Id.Value,
                a.OrganizationId,
                a.Code,
                a.AssetModelId.Value,
                a.SerialNumber,
                a.LicensePlate,
                a.ManufactureYear,
                a.Color,
                a.Status.ToString()))
            .ToList();

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchAssetsResponse(
            items,
            page,
            pageSize,
            totalItems,
            totalPages,
            page < totalPages,
            page > 1);
    }
}