using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Fuel Type is deactivated (soft-deleted).</summary>
public sealed class FuelTypeDeactivated : IDomainEvent
{
    /// <summary>Gets the FuelTypeId value.</summary>
    public FuelTypeId FuelTypeId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="FuelTypeDeactivated"/> class.</summary>
    public FuelTypeDeactivated(FuelTypeId fuelTypeId, Guid holdingId, DateTimeOffset occurredOn)
    {
        FuelTypeId = fuelTypeId;
        HoldingId = holdingId;
        OccurredOn = occurredOn;
    }
}
