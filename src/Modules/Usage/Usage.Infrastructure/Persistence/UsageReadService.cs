using MachineryManagerEnterprise.Usage.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUsageReadService"/> —
/// <c>AsNoTracking</c> query joining the owned <c>UsageEntry</c> table
/// back to its owning <c>UsageLedger</c> row (via the shadow
/// <c>UsageLedgerId</c> foreign key), same materialize-then-project
/// convention as <c>AssetRepository.SearchAsync</c>.
/// </summary>
public sealed class UsageReadService : IUsageReadService
{
    private readonly UsageDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UsageReadService"/> class.</summary>
    /// <param name="dbContext">The Usage module's persistence context.</param>
    public UsageReadService(UsageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<PagedResult<UsageEntryListItem>> ListByMeterDeviceAsync(
        Guid meterDeviceId,
        DateOnly? fromDate,
        DateOnly? toDate,
        Guid? projectId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var meterDeviceIdTyped = global::Usage.Domain.MeterDeviceId.From(meterDeviceId);

        var query =
            from entry in _dbContext.Set<global::Usage.Domain.UsageEntry>().AsNoTracking()
            join ledger in _dbContext.UsageLedgers.AsNoTracking()
                on EF.Property<Guid>(entry, "UsageLedgerId") equals ledger.Id.Value
            where entry.SourceMeterDeviceId == meterDeviceIdTyped
            select new { entry, ledger };

        if (fromDate is not null)
        {
            query = query.Where(x => x.entry.EntryDate >= fromDate.Value);
        }

        if (toDate is not null)
        {
            query = query.Where(x => x.entry.EntryDate <= toDate.Value);
        }

        if (projectId is not null)
        {
            query = query.Where(x => x.entry.ProjectId == projectId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        // Project to an anonymous shape first — Id.Value access on a
        // converted strongly-typed id is a client-side operation, so it
        // happens after materialization rather than inside the
        // server-translated Select (same reasoning as the
        // Select-projection caveat noted in AssetRepository.SearchAsync).
        var rows = await query
            .OrderByDescending(x => x.entry.EntryDate)
            .ThenByDescending(x => x.entry.ShiftIndex)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                LedgerId = x.ledger.Id,
                x.ledger.OwnerType,
                x.ledger.OwnerId,
                x.entry.EntryDate,
                x.entry.Kind,
                x.entry.ShiftIndex,
                x.entry.ShiftStartTime,
                x.entry.ShiftEndTime,
                x.entry.RawReadingValue,
                x.entry.OperationalUsageAmount,
                x.entry.OperatorId,
                x.entry.ProjectId,
                x.entry.Origin,
            })
            .ToListAsync(cancellationToken);

        var items = rows
            .Select(r => new UsageEntryListItem(
                r.LedgerId.Value,
                r.OwnerType,
                r.OwnerId,
                r.EntryDate,
                r.Kind,
                r.ShiftIndex,
                r.ShiftStartTime,
                r.ShiftEndTime,
                r.RawReadingValue,
                r.OperationalUsageAmount,
                r.OperatorId,
                r.ProjectId,
                r.Origin))
            .ToList();

        return new PagedResult<UsageEntryListItem>(items, totalCount, page, pageSize);
    }
}
