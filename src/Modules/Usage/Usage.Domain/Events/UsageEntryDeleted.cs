using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when the most recently registered entry is deleted (BR-049
/// — backward-only deletion; only permitted while the entry is not yet
/// frozen, BR-052/BR-053).
/// </summary>
public sealed class UsageEntryDeleted : IDomainEvent
{
    /// <summary>Gets the identifier of the Usage Ledger the deleted entry belonged to.</summary>
    public UsageLedgerId UsageLedgerId { get; }

    /// <summary>Gets the calendar date of the deleted entry.</summary>
    public DateOnly EntryDate { get; }

    /// <summary>Gets the Operational Usage the deleted entry had contributed, removed from the total as a result of this deletion.</summary>
    public decimal RemovedOperationalUsageAmount { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="UsageEntryDeleted"/> class.</summary>
    /// <param name="usageLedgerId">The identifier of the Usage Ledger the deleted entry belonged to.</param>
    /// <param name="entryDate">The calendar date of the deleted entry.</param>
    /// <param name="removedOperationalUsageAmount">The Operational Usage the deleted entry had contributed.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public UsageEntryDeleted(UsageLedgerId usageLedgerId, DateOnly entryDate, decimal removedOperationalUsageAmount, DateTimeOffset occurredOn)
    {
        UsageLedgerId = usageLedgerId;
        EntryDate = entryDate;
        RemovedOperationalUsageAmount = removedOperationalUsageAmount;
        OccurredOn = occurredOn;
    }
}
