using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Color is deactivated (soft-deleted).</summary>
public sealed class ColorDeactivated : IDomainEvent
{
    /// <summary>Gets the ColorId value.</summary>
    public ColorId ColorId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="ColorDeactivated"/> class.</summary>
    public ColorDeactivated(ColorId colorId, Guid holdingId, DateTimeOffset occurredOn)
    {
        ColorId = colorId;
        HoldingId = holdingId;
        OccurredOn = occurredOn;
    }
}
