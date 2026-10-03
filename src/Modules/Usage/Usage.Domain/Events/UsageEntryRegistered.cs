using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when a new entry is appended to a <see cref="UsageLedger"/>'s
/// timeline — a normal shift reading, or a shift auto-filled with zero
/// usage because the user did not report it (BR-044). Not raised for a
/// <see cref="UsageEntryKind.Rebase"/> entry; see <see cref="CounterRebased"/>.
/// </summary>
public sealed class UsageEntryRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the Usage Ledger this entry belongs to.</summary>
    public UsageLedgerId UsageLedgerId { get; }

    /// <summary>Gets the kind of the ledger's owner (Asset or Component).</summary>
    public UsageOwnerType OwnerType { get; }

    /// <summary>Gets the identifier of the ledger's owner.</summary>
    public Guid OwnerId { get; }

    /// <summary>Gets the unit this ledger tracks.</summary>
    public MeterReadingUnit Unit { get; }

    /// <summary>Gets the calendar date this entry belongs to.</summary>
    public DateOnly EntryDate { get; }

    /// <summary>Gets the Work Calendar shift index this entry reports on.</summary>
    public int ShiftIndex { get; }

    /// <summary>Gets whether this entry was entered by a user or auto-filled for a missed shift (BR-044).</summary>
    public UsageEntryOrigin Origin { get; }

    /// <summary>Gets the Operational Usage contributed by this entry.</summary>
    public decimal OperationalUsageAmount { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="UsageEntryRegistered"/> class.</summary>
    /// <param name="usageLedgerId">The identifier of the Usage Ledger this entry belongs to.</param>
    /// <param name="ownerType">The kind of the ledger's owner.</param>
    /// <param name="ownerId">The identifier of the ledger's owner.</param>
    /// <param name="unit">The unit this ledger tracks.</param>
    /// <param name="entryDate">The calendar date this entry belongs to.</param>
    /// <param name="shiftIndex">The Work Calendar shift index this entry reports on.</param>
    /// <param name="origin">Whether this entry was entered by a user or auto-filled.</param>
    /// <param name="operationalUsageAmount">The Operational Usage contributed by this entry.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public UsageEntryRegistered(
        UsageLedgerId usageLedgerId,
        UsageOwnerType ownerType,
        Guid ownerId,
        MeterReadingUnit unit,
        DateOnly entryDate,
        int shiftIndex,
        UsageEntryOrigin origin,
        decimal operationalUsageAmount,
        DateTimeOffset occurredOn)
    {
        UsageLedgerId = usageLedgerId;
        OwnerType = ownerType;
        OwnerId = ownerId;
        Unit = unit;
        EntryDate = entryDate;
        ShiftIndex = shiftIndex;
        Origin = origin;
        OperationalUsageAmount = operationalUsageAmount;
        OccurredOn = occurredOn;
    }
}
