using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain.Events;

/// <summary>Raised when a new Work Calendar is registered.</summary>
public sealed class WorkCalendarRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered calendar.</summary>
    public WorkCalendarId WorkCalendarId { get; }

    /// <summary>Gets the identifier of the Project this calendar governs.</summary>
    public Guid ProjectId { get; }

    /// <summary>Gets the name of the calendar.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkCalendarRegistered"/> class.</summary>
    public WorkCalendarRegistered(WorkCalendarId workCalendarId, Guid projectId, string name, DateTimeOffset occurredOn)
    {
        WorkCalendarId = workCalendarId;
        ProjectId = projectId;
        Name = name;
        OccurredOn = occurredOn;
    }
}