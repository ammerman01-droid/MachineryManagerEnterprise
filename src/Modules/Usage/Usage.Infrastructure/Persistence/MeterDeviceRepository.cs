using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IMeterDeviceRepository"/>.</summary>
public sealed class MeterDeviceRepository : IMeterDeviceRepository
{
    private readonly UsageDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="MeterDeviceRepository"/> class.</summary>
    /// <param name="dbContext">The Usage module's persistence context.</param>
    public MeterDeviceRepository(UsageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<global::Usage.Domain.MeterDevice?> GetByIdAsync(
        global::Usage.Domain.MeterDeviceId id, CancellationToken cancellationToken = default) =>
        _dbContext.MeterDevices.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(global::Usage.Domain.MeterDevice aggregate) => _dbContext.MeterDevices.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::Usage.Domain.MeterDevice aggregate) => _dbContext.MeterDevices.Update(aggregate);

    /// <inheritdoc />
    public void Remove(global::Usage.Domain.MeterDevice aggregate) => _dbContext.MeterDevices.Remove(aggregate);

    /// <inheritdoc />
    public async Task<SearchMeterDevicesResponse> SearchAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.MeterDevices
            .AsNoTracking()
            .Where(d => d.OrganizationId == organizationId);

        var totalCount = await query.CountAsync(cancellationToken);

        // Two-step projection — materialize the raw typed values first, then
        // convert to display strings client-side, rather than calling
        // .ToString() on a converted enum inside the server-translated
        // Select (same reasoning as UsageReadService.ListByMeterDeviceAsync).
        var rows = await query
            .OrderBy(d => d.Status)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new
            {
                d.Id,
                d.OrganizationId,
                d.Unit,
                d.Status,
                d.OwnerType,
                d.OwnerId,
                d.DailyCapOverride,
            })
            .ToListAsync(cancellationToken);

        var items = rows
            .Select(r => new MeterDeviceListItemDto(
                r.Id.Value,
                r.OrganizationId,
                r.Unit.ToString(),
                r.Status.ToString(),
                r.OwnerType?.ToString(),
                r.OwnerId,
                r.DailyCapOverride))
            .ToList();

        return new SearchMeterDevicesResponse(items, totalCount);
    }
}
