using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when an administrator moves a Usage Ledger's freeze boundary
/// forward (BR-052). Entries dated on or before
/// <see cref="FrozenUpToDate"/> can no longer be registered, corrected,
/// or deleted by project-level users (BR-053).
/// </summary>
public sealed class UsageFrozen : IDomainEvent
{
    /// <summary>Gets the identifier of the Usage Ledger that was frozen.</summary>
    public UsageLedgerId UsageLedgerId { get; }

    /// <summary>Gets the new freeze boundary (inclusive).</summary>
    public DateOnly FrozenUpToDate { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="UsageFrozen"/> class.</summary>
    /// <param name="usageLedgerId">The identifier of the Usage Ledger that was frozen.</param>
    /// <param name="frozenUpToDate">The new freeze boundary (inclusive).</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public UsageFrozen(UsageLedgerId usageLedgerId, DateOnly frozenUpToDate, DateTimeOffset occurredOn)
    {
        UsageLedgerId = usageLedgerId;
        FrozenUpToDate = frozenUpToDate;
        OccurredOn = occurredOn;
    }
}
