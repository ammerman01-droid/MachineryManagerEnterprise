using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Consumption.Infrastructure.Persistence;

/// <inheritdoc cref="ILubricantOverflowReportRepository" />
public sealed class LubricantOverflowReportRepository : ILubricantOverflowReportRepository
{
    private readonly ConsumptionDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="LubricantOverflowReportRepository"/> class.</summary>
    public LubricantOverflowReportRepository(ConsumptionDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<LubricantOverflowReport?> GetByIdAsync(LubricantOverflowReportId id, CancellationToken cancellationToken = default) =>
        _dbContext.LubricantOverflowReports.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<LubricantOverflowReport>> GetByAssetAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        var reports = await _dbContext.LubricantOverflowReports
            .Where(r => r.AssetId == assetId)
            .OrderByDescending(r => r.ReportDate)
            .ToListAsync(cancellationToken);

        return reports;
    }

    /// <inheritdoc />
    public Task<bool> IsLubricantTypeInUseAsync(Guid lubricantTypeId, CancellationToken cancellationToken = default) =>
        _dbContext.LubricantOverflowReports
            .AsNoTracking()
            .AnyAsync(r => r.Lines.Any(l => l.LubricantTypeId == lubricantTypeId), cancellationToken);

    /// <inheritdoc />
    public Task<bool> IsOverflowComponentInUseAsync(Guid overflowComponentId, CancellationToken cancellationToken = default) =>
        _dbContext.LubricantOverflowReports
            .AsNoTracking()
            .AnyAsync(r => r.Lines.Any(l => l.OverflowComponentId == overflowComponentId), cancellationToken);

    /// <inheritdoc />
    public void Add(LubricantOverflowReport report) => _dbContext.LubricantOverflowReports.Add(report);

    /// <inheritdoc />
    public void Update(LubricantOverflowReport report) => _dbContext.LubricantOverflowReports.Update(report);

    /// <inheritdoc />
    public void Remove(LubricantOverflowReport report) => _dbContext.LubricantOverflowReports.Remove(report);
}
