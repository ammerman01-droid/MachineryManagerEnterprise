using Consumption.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Consumption.Domain;

/// <summary>
/// Aggregate Root holding, for a single Organization, the date threshold
/// below (and on) which Lubricant Overflow Reports (and, in future,
/// other Consumption records) can no longer be edited or deleted. Only
/// an Organization Administrator may change it. Exactly one instance
/// exists per Organization, created lazily the first time the threshold
/// is set (chat, 2026-09-16).
/// </summary>
public sealed class ConsumptionFreezeSetting : AggregateRoot<ConsumptionFreezeSettingId>
{
    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the freeze threshold date. Reports with a Report Date on or before this date can no longer be edited or deleted.</summary>
    public DateOnly ThresholdDate { get; private set; }

    private ConsumptionFreezeSetting()
    {
    }

    private ConsumptionFreezeSetting(ConsumptionFreezeSettingId id, Guid organizationId, DateOnly thresholdDate)
        : base(id)
    {
        OrganizationId = organizationId;
        ThresholdDate = thresholdDate;
    }

    /// <summary>Creates the freeze setting for an Organization for the first time.</summary>
    public static ConsumptionFreezeSetting Create(Guid organizationId, DateOnly thresholdDate, IDateTimeProvider dateTimeProvider)
    {
        var setting = new ConsumptionFreezeSetting(ConsumptionFreezeSettingId.New(), organizationId, thresholdDate);

        setting.RaiseDomainEvent(new ConsumptionFreezeThresholdSet(organizationId, thresholdDate, dateTimeProvider.UtcNow));

        return setting;
    }

    /// <summary>Moves the freeze threshold to a new date.</summary>
    public void SetThreshold(DateOnly thresholdDate, IDateTimeProvider dateTimeProvider)
    {
        ThresholdDate = thresholdDate;
        RaiseDomainEvent(new ConsumptionFreezeThresholdSet(OrganizationId, thresholdDate, dateTimeProvider.UtcNow));
    }
}
