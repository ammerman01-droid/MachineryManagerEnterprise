using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a previously deactivated Fuel Type is reactivated.</summary>
public sealed class FuelTypeActivated : IDomainEvent
{
    /// <summary>Gets the FuelTypeId value.</summary>
    public FuelTypeId FuelTypeId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="FuelTypeActivated"/> class.</summary>
    public FuelTypeActivated(FuelTypeId fuelTypeId, Guid holdingId, DateTimeOffset occurredOn)
    {
        FuelTypeId = fuelTypeId;
        HoldingId = holdingId;
        OccurredOn = occurredOn;
    }
}
