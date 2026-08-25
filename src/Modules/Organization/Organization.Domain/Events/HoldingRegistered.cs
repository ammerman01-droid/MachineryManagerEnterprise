using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when a new Holding is registered.</summary>
public sealed class HoldingRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered holding.</summary>
    public HoldingId HoldingId { get; }

    /// <summary>Gets the name of the registered holding.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="HoldingRegistered"/> class.</summary>
    public HoldingRegistered(HoldingId holdingId, string name, DateTimeOffset occurredOn)
    {
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}