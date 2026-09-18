using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain.Events;

/// <summary>Raised when a Work Pattern is replaced by a new version (BR-018-010).</summary>
public sealed class WorkPatternSuperseded : IDomainEvent
{
    /// <summary>Gets the identifier of the owning calendar.</summary>
    public WorkCalendarId WorkCalendarId { get; }

    /// <summary>Gets the identifier of the pattern that was superseded (now retained as historical).</summary>
    public WorkPatternId SupersededWorkPatternId { get; }

    /// <summary>Gets the identifier of the new pattern version that replaced it.</summary>
    public WorkPatternId NewWorkPatternId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkPatternSuperseded"/> class.</summary>
    public WorkPatternSuperseded(
        WorkCalendarId workCalendarId, WorkPatternId supersededWorkPatternId, WorkPatternId newWorkPatternId, DateTimeOffset occurredOn)
    {
        WorkCalendarId = workCalendarId;
        SupersededWorkPatternId = supersededWorkPatternId;
        NewWorkPatternId = newWorkPatternId;
        OccurredOn = occurredOn;
    }
}