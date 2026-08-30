using MachineryManager.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Represents the ColorRegistered type.</summary>
public sealed class ColorRegistered : IDomainEvent
{
/// <summary>Gets the ColorId value.</summary>
    public ColorId ColorId { get; }
/// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }
/// <summary>Gets the Name value.</summary>
    public string Name { get; }
/// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

/// <summary>Initializes a new instance of the <see cref="ColorRegistered"/> class.</summary>
    public ColorRegistered(ColorId colorId, Guid holdingId, string name, DateTimeOffset occurredOn)
    {
        ColorId = colorId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}