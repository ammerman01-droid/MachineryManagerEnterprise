using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Consumption.Infrastructure.Persistence;

/// <inheritdoc cref="IConsumptionFreezeSettingRepository" />
public sealed class ConsumptionFreezeSettingRepository : IConsumptionFreezeSettingRepository
{
    private readonly ConsumptionDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="ConsumptionFreezeSettingRepository"/> class.</summary>
    public ConsumptionFreezeSettingRepository(ConsumptionDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<ConsumptionFreezeSetting?> GetByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default) =>
        _dbContext.ConsumptionFreezeSettings.FirstOrDefaultAsync(s => s.OrganizationId == organizationId, cancellationToken);

    /// <inheritdoc />
    public void Add(ConsumptionFreezeSetting setting) => _dbContext.ConsumptionFreezeSettings.Add(setting);

    /// <inheritdoc />
    public void Update(ConsumptionFreezeSetting setting) => _dbContext.ConsumptionFreezeSettings.Update(setting);
}
