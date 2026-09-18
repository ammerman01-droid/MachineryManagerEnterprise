using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain.Events;

/// <summary>Raised when a Work Pattern is manually retired directly to Historical.</summary>
public sealed class WorkPatternRetired : IDomainEvent
{
    /// <summary>Gets the identifier of the owning calendar.</summary>
    public WorkCalendarId WorkCalendarId { get; }

    /// <summary>Gets the identifier of the retired pattern.</summary>
    public WorkPatternId WorkPatternId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkPatternRetired"/> class.</summary>
    public WorkPatternRetired(WorkCalendarId workCalendarId, WorkPatternId workPatternId, DateTimeOffset occurredOn)
    {
        WorkCalendarId = workCalendarId;
        WorkPatternId = workPatternId;
        OccurredOn = occurredOn;
    }
}