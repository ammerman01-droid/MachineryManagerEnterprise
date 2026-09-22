using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Fuel Type's name, price, or kind is updated.</summary>
public sealed class FuelTypeUpdated : IDomainEvent
{
    /// <summary>Gets the FuelTypeId value.</summary>
    public FuelTypeId FuelTypeId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the new Name value.</summary>
    public string Name { get; }

    /// <summary>Gets the new Price value.</summary>
    public long Price { get; }

    /// <summary>Gets the new Kind value.</summary>
    public FuelKind Kind { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="FuelTypeUpdated"/> class.</summary>
    public FuelTypeUpdated(FuelTypeId fuelTypeId, Guid holdingId, string name, long price, FuelKind kind, DateTimeOffset occurredOn)
    {
        FuelTypeId = fuelTypeId;
        HoldingId = holdingId;
        Name = name;
        Price = price;
        Kind = kind;
        OccurredOn = occurredOn;
    }
}
