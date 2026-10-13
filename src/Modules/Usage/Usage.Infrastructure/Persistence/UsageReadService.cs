using MachineryManagerEnterprise.Usage.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUsageReadService"/> —
/// <c>AsNoTracking</c> query joining the owned <c>UsageEntry</c> table
/// back to its owning <c>UsageLedger</c> row (via the shadow
/// <c>UsageLedgerId</c> foreign key), same materialize-then-project
/// convention as <c>AssetRepository.SearchAsync</c>. The two public
/// methods share the same shape of query but are kept as separate,
/// fully-written-out LINQ pipelines rather than factored through a
/// shared private helper — C# anonymous types are not nominal, so a
/// helper spanning both methods' projections would have to fall back
/// to <c>dynamic</c> or a named DTO, neither of which EF Core can
/// translate as cleanly as two plain, directly-composed queries.
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
            from ledger in _dbContext.UsageLedgers.AsNoTracking()
            from entry in ledger.Entries
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

        // Project to an anonymous shape first — Id.Value access on a converted strongly-typed id is
        // a client-side operation, so it happens after materialization rather than inside the
        // server-translated Select (same reasoning as the Select-projection caveat noted in
        // AssetRepository.SearchAsync).
        var rows = await query
            .OrderByDescending(x => x.entry.EntryDate)
            .ThenByDescending(x => x.entry.ShiftIndex)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                LedgerId = x.ledger.Id,
                x.entry.SourceMeterDeviceId,
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
                r.SourceMeterDeviceId.Value,
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

    /// <inheritdoc />
    public async Task<PagedResult<UsageEntryListItem>> ListByOrganizationAsync(
        Guid organizationId,
        DateOnly? fromDate,
        DateOnly? toDate,
        Guid? projectId,
        Guid? meterDeviceId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query =
            from ledger in _dbContext.UsageLedgers.AsNoTracking()
            from entry in ledger.Entries
            join device in _dbContext.MeterDevices.AsNoTracking()
                on entry.SourceMeterDeviceId equals device.Id
            where device.OrganizationId == organizationId
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

        if (meterDeviceId is not null)
        {
            var meterDeviceIdTyped = global::Usage.Domain.MeterDeviceId.From(meterDeviceId.Value);
            query = query.Where(x => x.entry.SourceMeterDeviceId == meterDeviceIdTyped);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var rows = await query
            .OrderByDescending(x => x.entry.EntryDate)
            .ThenByDescending(x => x.entry.ShiftIndex)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                LedgerId = x.ledger.Id,
                x.entry.SourceMeterDeviceId,
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
                r.SourceMeterDeviceId.Value,
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
