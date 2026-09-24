using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Dtos;
using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.SearchFuelConsumptionsByAsset;
using MachineryManagerEnterprise.Consumption.Domain;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Consumption.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IFuelConsumptionRepository"/>.</summary>
public sealed class FuelConsumptionRepository : IFuelConsumptionRepository
{
    private readonly ConsumptionDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="FuelConsumptionRepository"/> class.</summary>
    /// <param name="dbContext">The Consumption module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public FuelConsumptionRepository(ConsumptionDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<FuelConsumption?> GetByIdAsync(FuelConsumptionId id, CancellationToken cancellationToken = default) =>
        _dbContext.FuelConsumptions.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(FuelConsumption aggregate) => _dbContext.FuelConsumptions.Add(aggregate);

    /// <inheritdoc />
    public void Update(FuelConsumption aggregate) => _dbContext.FuelConsumptions.Update(aggregate);

    /// <inheritdoc />
    public void Remove(FuelConsumption aggregate) => _dbContext.FuelConsumptions.Remove(aggregate);

    /// <inheritdoc />
    public Task<FuelConsumption?> GetPreviousAsync(
        Guid assetId,
        DateTimeOffset beforeUtc,
        Guid? excludingId,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.FuelConsumptions
            .AsNoTracking()
            .Where(f => f.AssetId == assetId && f.RecordedAtUtc < beforeUtc);

        if (excludingId is { } id)
        {
            var excludedFuelConsumptionId = FuelConsumptionId.From(id);
            query = query.Where(f => f.Id != excludedFuelConsumptionId);
        }

        return query
            .OrderByDescending(f => f.RecordedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<FuelConsumption?> GetNextAsync(
        Guid assetId,
        DateTimeOffset afterUtc,
        Guid? excludingId,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.FuelConsumptions
            .AsNoTracking()
            .Where(f => f.AssetId == assetId && f.RecordedAtUtc > afterUtc);

        if (excludingId is { } id)
        {
            var excludedFuelConsumptionId = FuelConsumptionId.From(id);
            query = query.Where(f => f.Id != excludedFuelConsumptionId);
        }

        return query
            .OrderBy(f => f.RecordedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<SearchFuelConsumptionsByAssetResponse> SearchByAssetAsync(
        Guid assetId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.FuelConsumptions
            .AsNoTracking()
            .Where(f => f.AssetId == assetId);

        var totalItems = await query.CountAsync(cancellationToken);

        // Materialize entities first, map to DTO in memory via Mapster's
        // IMapper afterward — avoids the EF Core 10 Select-projection
        // translation issue documented for field-backed collection/
        // value-object properties (mirrors AssetRepository.SearchAsync,
        // chat, 2026-08-25).
        var entities = await query
            .OrderByDescending(f => f.RecordedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = _mapper.Map<List<FuelConsumptionDto>>(entities);

        return new SearchFuelConsumptionsByAssetResponse(items, page, pageSize, totalItems);
    }
}
