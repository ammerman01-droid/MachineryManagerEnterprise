using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Color's display name is updated.</summary>
public sealed class ColorUpdated : IDomainEvent
{
    /// <summary>Gets the ColorId value.</summary>
    public ColorId ColorId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the new Name value.</summary>
    public string Name { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="ColorUpdated"/> class.</summary>
    public ColorUpdated(ColorId colorId, Guid holdingId, string name, DateTimeOffset occurredOn)
    {
        ColorId = colorId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}
