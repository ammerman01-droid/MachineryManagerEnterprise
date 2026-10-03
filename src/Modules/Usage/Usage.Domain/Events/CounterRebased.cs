using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when a Usage Ledger's counter is rebased — the device was
/// replaced or its counter reset (chat, 2026-09-29). Purely
/// informational: a rebase never contributes Operational Usage on its
/// own, so no reporting recalculation is triggered by this event.
/// </summary>
public sealed class CounterRebased : IDomainEvent
{
    /// <summary>Gets the identifier of the Usage Ledger that was rebased.</summary>
    public UsageLedgerId UsageLedgerId { get; }

    /// <summary>Gets the date the rebase was recorded on.</summary>
    public DateOnly EntryDate { get; }

    /// <summary>Gets the new device the Ledger now reads from.</summary>
    public MeterDeviceId NewSourceMeterDeviceId { get; }

    /// <summary>Gets the new baseline raw reading value.</summary>
    public decimal NewRawReadingValue { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="CounterRebased"/> class.</summary>
    /// <param name="usageLedgerId">The identifier of the Usage Ledger that was rebased.</param>
    /// <param name="entryDate">The date the rebase was recorded on.</param>
    /// <param name="newSourceMeterDeviceId">The new device the Ledger now reads from.</param>
    /// <param name="newRawReadingValue">The new baseline raw reading value.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public CounterRebased(UsageLedgerId usageLedgerId, DateOnly entryDate, MeterDeviceId newSourceMeterDeviceId, decimal newRawReadingValue, DateTimeOffset occurredOn)
    {
        UsageLedgerId = usageLedgerId;
        EntryDate = entryDate;
        NewSourceMeterDeviceId = newSourceMeterDeviceId;
        NewRawReadingValue = newRawReadingValue;
        OccurredOn = occurredOn;
    }
}
