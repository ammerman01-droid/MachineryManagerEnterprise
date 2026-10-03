using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when a Meter Device is removed from its current owner. Per
/// BR-013, its reading history is never deleted — only the current
/// owner link is cleared, so the device can later be re-installed
/// (possibly on a different Asset or Component) while keeping its
/// full history.
/// </summary>
public sealed class MeterRemoved : IDomainEvent
{
    /// <summary>Gets the identifier of the removed Meter Device.</summary>
    public MeterDeviceId MeterDeviceId { get; }

    /// <summary>Gets the kind of owner the device was removed from.</summary>
    public UsageOwnerType PreviousOwnerType { get; }

    /// <summary>Gets the identifier of the owner the device was removed from.</summary>
    public Guid PreviousOwnerId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="MeterRemoved"/> class.</summary>
    /// <param name="meterDeviceId">The identifier of the removed Meter Device.</param>
    /// <param name="previousOwnerType">The kind of owner the device was removed from.</param>
    /// <param name="previousOwnerId">The identifier of the owner the device was removed from.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public MeterRemoved(MeterDeviceId meterDeviceId, UsageOwnerType previousOwnerType, Guid previousOwnerId, DateTimeOffset occurredOn)
    {
        MeterDeviceId = meterDeviceId;
        PreviousOwnerType = previousOwnerType;
        PreviousOwnerId = previousOwnerId;
        OccurredOn = occurredOn;
    }
}
