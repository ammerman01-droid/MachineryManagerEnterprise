using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain.Events;

/// <summary>
/// Raised when a removed Meter Device reaches its final, Archived
/// state. Its full reading history remains accessible; only its
/// lifecycle is closed.
/// <para>
/// NOTE (chat, 2026-09-12): this event is not part of the original
/// Domain Events catalog (Section 4.7), which lists only
/// MeterInstalled, MeterRemoved, MeterFailureDetected and
/// MeterReadingRecorded for this aggregate. It is added here for
/// symmetry with the already-catalogued <c>ArchiveMeter</c> command
/// (Section 5.3) and should be added to the official catalog on
/// review.
/// </para>
/// </summary>
public sealed class MeterArchived : IDomainEvent
{
    /// <summary>Gets the identifier of the archived Meter Device.</summary>
    public MeterDeviceId MeterDeviceId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="MeterArchived"/> class.</summary>
    /// <param name="meterDeviceId">The identifier of the archived Meter Device.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public MeterArchived(MeterDeviceId meterDeviceId, DateTimeOffset occurredOn)
    {
        MeterDeviceId = meterDeviceId;
        OccurredOn = occurredOn;
    }
}
