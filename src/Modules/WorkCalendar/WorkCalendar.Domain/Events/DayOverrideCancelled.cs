using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain.Events;

/// <summary>Raised when a Day Override is cancelled.</summary>
public sealed class DayOverrideCancelled : IDomainEvent
{
    /// <summary>Gets the identifier of the owning calendar.</summary>
    public WorkCalendarId WorkCalendarId { get; }

    /// <summary>Gets the identifier of the cancelled override.</summary>
    public DayOverrideId DayOverrideId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="DayOverrideCancelled"/> class.</summary>
    public DayOverrideCancelled(WorkCalendarId workCalendarId, DayOverrideId dayOverrideId, DateTimeOffset occurredOn)
    {
        WorkCalendarId = workCalendarId;
        DayOverrideId = dayOverrideId;
        OccurredOn = occurredOn;
    }
}