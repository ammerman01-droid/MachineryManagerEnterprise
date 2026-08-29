using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when a new Color option is registered.</summary>
public sealed class ColorRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered color.</summary>
    public ColorId ColorId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the name of the color.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="ColorRegistered"/> class.</summary>
    public ColorRegistered(ColorId colorId, Guid organizationId, string name, DateTimeOffset occurredOn)
    {
        ColorId = colorId;
        OrganizationId = organizationId;
        Name = name;
        OccurredOn = occurredOn;
    }
}