using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a previously deactivated Color is reactivated.</summary>
public sealed class ColorActivated : IDomainEvent
{
    /// <summary>Gets the ColorId value.</summary>
    public ColorId ColorId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="ColorActivated"/> class.</summary>
    public ColorActivated(ColorId colorId, Guid holdingId, DateTimeOffset occurredOn)
    {
        ColorId = colorId;
        HoldingId = holdingId;
        OccurredOn = occurredOn;
    }
}
