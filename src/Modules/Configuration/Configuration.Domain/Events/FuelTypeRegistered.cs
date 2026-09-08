using MachineryManager.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a new Fuel Type is registered.</summary>
public sealed class FuelTypeRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered fuel type.</summary>
    public FuelTypeId FuelTypeId { get; }

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the display name of the fuel type.</summary>
    public string Name { get; }

    /// <summary>Gets the price at the time of registration.</summary>
    public long Price { get; }

    /// <summary>Gets the fixed fuel-kind classification of the registered fuel type.</summary>
    public FuelKind Kind { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="FuelTypeRegistered"/> class.</summary>
    /// <param name="fuelTypeId">The identifier of the registered fuel type.</param>
    /// <param name="holdingId">The identifier of the owning Holding.</param>
    /// <param name="name">The display name of the fuel type.</param>
    /// <param name="price">The price at the time of registration.</param>
    /// <param name="kind">The fixed fuel-kind classification of the registered fuel type.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public FuelTypeRegistered(
        FuelTypeId fuelTypeId, Guid holdingId, string name, long price, FuelKind kind, DateTimeOffset occurredOn)
    {
        FuelTypeId = fuelTypeId;
        HoldingId = holdingId;
        Name = name;
        Price = price;
        Kind = kind;
        OccurredOn = occurredOn;
    }
}