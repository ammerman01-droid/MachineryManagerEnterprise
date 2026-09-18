using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain.Events;

/// <summary>Raised when a new Day Override is added to a Work Calendar.</summary>
public sealed class DayOverrideAdded : IDomainEvent
{
    /// <summary>Gets the identifier of the owning calendar.</summary>
    public WorkCalendarId WorkCalendarId { get; }

    /// <summary>Gets the identifier of the added override.</summary>
    public DayOverrideId DayOverrideId { get; }

    /// <summary>Gets the calendar date the override applies to.</summary>
    public DateOnly Date { get; }

    /// <summary>Gets the category of the override.</summary>
    public DayOverrideType Type { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="DayOverrideAdded"/> class.</summary>
    public DayOverrideAdded(
        WorkCalendarId workCalendarId, DayOverrideId dayOverrideId, DateOnly date, DayOverrideType type, DateTimeOffset occurredOn)
    {
        WorkCalendarId = workCalendarId;
        DayOverrideId = dayOverrideId;
        Date = date;
        Type = type;
        OccurredOn = occurredOn;
    }
}