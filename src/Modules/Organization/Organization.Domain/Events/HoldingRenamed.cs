using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when a Holding's name is changed.</summary>
public sealed class HoldingRenamed : IDomainEvent
{
    /// <summary>Gets the identifier of the renamed holding.</summary>
    public HoldingId HoldingId { get; }

    /// <summary>Gets the new name of the holding.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="HoldingRenamed"/> class.</summary>
    public HoldingRenamed(HoldingId holdingId, string name, DateTimeOffset occurredOn)
    {
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}