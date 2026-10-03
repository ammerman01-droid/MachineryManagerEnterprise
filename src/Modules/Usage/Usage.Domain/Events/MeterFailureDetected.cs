using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when a Meter Device is marked as malfunctioning. Per BR-014,
/// this never invalidates previously calculated Operational Usage —
/// history already derived from valid readings stands.
/// </summary>
public sealed class MeterFailureDetected : IDomainEvent
{
    /// <summary>Gets the identifier of the failed Meter Device.</summary>
    public MeterDeviceId MeterDeviceId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="MeterFailureDetected"/> class.</summary>
    /// <param name="meterDeviceId">The identifier of the failed Meter Device.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public MeterFailureDetected(MeterDeviceId meterDeviceId, DateTimeOffset occurredOn)
    {
        MeterDeviceId = meterDeviceId;
        OccurredOn = occurredOn;
    }
}
