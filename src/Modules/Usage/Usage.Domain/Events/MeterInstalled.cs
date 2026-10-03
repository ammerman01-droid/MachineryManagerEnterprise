using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>Raised when a Meter Device is installed on an owner (an Asset or a Tracked Component).</summary>
public sealed class MeterInstalled : IDomainEvent
{
    /// <summary>Gets the identifier of the installed Meter Device.</summary>
    public MeterDeviceId MeterDeviceId { get; }

    /// <summary>Gets the kind of owner the device was installed on.</summary>
    public UsageOwnerType OwnerType { get; }

    /// <summary>Gets the identifier of the owner the device was installed on.</summary>
    public Guid OwnerId { get; }

    /// <summary>Gets the unit resolved from the owner and now established on this device (chat, 2026-09-20).</summary>
    public MeterReadingUnit Unit { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="MeterInstalled"/> class.</summary>
    /// <param name="meterDeviceId">The identifier of the installed Meter Device.</param>
    /// <param name="ownerType">The kind of owner the device was installed on.</param>
    /// <param name="ownerId">The identifier of the owner the device was installed on.</param>
    /// <param name="unit">The unit resolved from the owner and now established on this device.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public MeterInstalled(MeterDeviceId meterDeviceId, UsageOwnerType ownerType, Guid ownerId, MeterReadingUnit unit, DateTimeOffset occurredOn)
    {
        MeterDeviceId = meterDeviceId;
        OwnerType = ownerType;
        OwnerId = ownerId;
        Unit = unit;
        OccurredOn = occurredOn;
    }
}
