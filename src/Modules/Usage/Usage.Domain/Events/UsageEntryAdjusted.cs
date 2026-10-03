using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when an already-registered entry's Operational Usage
/// changes — either because it was directly corrected via
/// <see cref="UsageLedger.CorrectEntryReading"/>, or because the very
/// next entry's delta had to be recalculated as a result (BR-051).
/// Downstream modules that already consumed <see cref="UsageEntryRegistered"/>
/// for this entry (Preventive Maintenance, Reporting, Forecasting)
/// subscribe to this event to re-run their own calculations.
/// </summary>
public sealed class UsageEntryAdjusted : IDomainEvent
{
    /// <summary>Gets the identifier of the Usage Ledger this entry belongs to.</summary>
    public UsageLedgerId UsageLedgerId { get; }

    /// <summary>Gets the kind of the ledger's owner (Asset or Component).</summary>
    public UsageOwnerType OwnerType { get; }

    /// <summary>Gets the identifier of the ledger's owner.</summary>
    public Guid OwnerId { get; }

    /// <summary>Gets the calendar date of the entry that was adjusted.</summary>
    public DateOnly EntryDate { get; }

    /// <summary>Gets the Work Calendar shift index of the entry that was adjusted.</summary>
    public int ShiftIndex { get; }

    /// <summary>Gets the entry's new Operational Usage, after adjustment.</summary>
    public decimal NewOperationalUsageAmount { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="UsageEntryAdjusted"/> class.</summary>
    /// <param name="usageLedgerId">The identifier of the Usage Ledger this entry belongs to.</param>
    /// <param name="ownerType">The kind of the ledger's owner.</param>
    /// <param name="ownerId">The identifier of the ledger's owner.</param>
    /// <param name="entryDate">The calendar date of the entry that was adjusted.</param>
    /// <param name="shiftIndex">The Work Calendar shift index of the entry that was adjusted.</param>
    /// <param name="newOperationalUsageAmount">The entry's new Operational Usage.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public UsageEntryAdjusted(
        UsageLedgerId usageLedgerId,
        UsageOwnerType ownerType,
        Guid ownerId,
        DateOnly entryDate,
        int shiftIndex,
        decimal newOperationalUsageAmount,
        DateTimeOffset occurredOn)
    {
        UsageLedgerId = usageLedgerId;
        OwnerType = ownerType;
        OwnerId = ownerId;
        EntryDate = entryDate;
        ShiftIndex = shiftIndex;
        NewOperationalUsageAmount = newOperationalUsageAmount;
        OccurredOn = occurredOn;
    }
}
