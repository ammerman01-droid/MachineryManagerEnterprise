using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Consumption.Infrastructure;

/// <inheritdoc cref="IConsumptionUsageLookupService" />
public sealed class ConsumptionUsageLookupService : IConsumptionUsageLookupService
{
    private readonly Persistence.ConsumptionDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="ConsumptionUsageLookupService"/> class.</summary>
    public ConsumptionUsageLookupService(Persistence.ConsumptionDbContext dbContext) => _dbContext = dbContext;

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
}
