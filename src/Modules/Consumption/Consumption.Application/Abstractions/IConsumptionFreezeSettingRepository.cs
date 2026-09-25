using Consumption.Domain;

namespace MachineryManagerEnterprise.Consumption.Application.Abstractions;

/// <summary>Repository for the <see cref="ConsumptionFreezeSetting"/> aggregate.</summary>
public interface IConsumptionFreezeSettingRepository
{
    /// <summary>Retrieves the freeze setting for the given Organization, or <c>null</c> if never set.</summary>
    Task<ConsumptionFreezeSetting?> GetByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default);

    /// <summary>Adds a new freeze setting.</summary>
    void Add(ConsumptionFreezeSetting setting);

    /// <summary>Marks an existing freeze setting as updated.</summary>
    void Update(ConsumptionFreezeSetting setting);
}
