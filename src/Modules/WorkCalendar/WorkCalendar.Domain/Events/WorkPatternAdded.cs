using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain.Events;

/// <summary>Raised when a new Work Pattern is added to a Work Calendar.</summary>
public sealed class WorkPatternAdded : IDomainEvent
{
    /// <summary>Gets the identifier of the owning calendar.</summary>
    public WorkCalendarId WorkCalendarId { get; }

    /// <summary>Gets the identifier of the added pattern.</summary>
    public WorkPatternId WorkPatternId { get; }

    /// <summary>Gets the first date the pattern applies to.</summary>
    public DateOnly StartDate { get; }

    /// <summary>Gets the last date the pattern applies to.</summary>
    public DateOnly EndDate { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkPatternAdded"/> class.</summary>
    public WorkPatternAdded(
        WorkCalendarId workCalendarId, WorkPatternId workPatternId, DateOnly startDate, DateOnly endDate, DateTimeOffset occurredOn)
    {
        WorkCalendarId = workCalendarId;
        WorkPatternId = workPatternId;
        StartDate = startDate;
        EndDate = endDate;
        OccurredOn = occurredOn;
    }
}