using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUsageCapPolicy"/>, backed by
/// the small <see cref="UsageCapSetting"/> table this module owns (see
/// its remarks for why this is plain Infrastructure state rather than
/// a Domain concept).
/// </summary>
public sealed class UsageCapPolicy : IUsageCapPolicy
{
    private readonly UsageDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UsageCapPolicy"/> class.</summary>
    /// <param name="dbContext">The Usage module's persistence context.</param>
    public UsageCapPolicy(UsageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<decimal> GetDefaultDailyCapAsync(MeterReadingUnit unit, CancellationToken cancellationToken = default)
    {
        var setting = await _dbContext.Set<UsageCapSetting>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Unit == unit, cancellationToken);

        // Defensive fallback only — the seed data in
        // UsageCapSettingConfiguration always inserts a row for every
        // MeterReadingUnit member, so this should be unreachable in
        // practice.
        return setting?.DefaultDailyCap ?? (unit == MeterReadingUnit.Hour ? 24m : 1000m);
    }
}
