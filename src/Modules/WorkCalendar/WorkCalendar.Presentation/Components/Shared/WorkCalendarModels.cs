namespace MachineryManagerEnterprise.WorkCalendar.Presentation.Components.Shared;

/// <summary>Client-side projection of the API's <c>BreakDto</c>.</summary>
public sealed class BreakItem
{
    /// <summary>Gets or sets the time the break starts.</summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>Gets or sets the time the break ends.</summary>
    public TimeOnly EndTime { get; set; }
}

/// <summary>Client-side projection of the API's <c>ShiftDto</c>.</summary>
public sealed class ShiftItem
{
    /// <summary>Gets or sets the time the shift starts.</summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>Gets or sets the time the shift ends.</summary>
    public TimeOnly EndTime { get; set; }

    /// <summary>Gets or sets the unpaid breaks within this shift.</summary>
    public List<BreakItem> Breaks { get; set; } = new();
}

/// <summary>Client-side projection of the API's <c>DayScheduleDto</c>, and the editable shape used while building a request.</summary>
public sealed class DayScheduleItem
{
    /// <summary>Gets or sets whether this day has at least one shift.</summary>
    public bool IsWorkingDay { get; set; }

    /// <summary>Gets or sets the shifts worked on this day.</summary>
    public List<ShiftItem> Shifts { get; set; } = new();
}

/// <summary>Client-side projection of the API's <c>WorkPatternDto</c>.</summary>
public sealed class WorkPatternItem
{
    /// <summary>Gets or sets the Work Pattern's identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the first date this pattern applies to.</summary>
    public DateOnly StartDate { get; set; }

    /// <summary>Gets or sets the last date this pattern applies to.</summary>
    public DateOnly EndDate { get; set; }

    /// <summary>Gets or sets the lifecycle status ("Draft", "Active", "Superseded", or "Historical").</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Gets or sets the template schedule for each day of the week.</summary>
    public Dictionary<DayOfWeek, DayScheduleItem> WeeklySchedule { get; set; } = new();
}

/// <summary>Client-side projection of the API's <c>DayOverrideDto</c>.</summary>
public sealed class DayOverrideItem
{
    /// <summary>Gets or sets the Day Override's identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the calendar date this override applies to.</summary>
    public DateOnly Date { get; set; }

    /// <summary>Gets or sets the category of this override ("Holiday", "Working", or "Modified").</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Gets or sets the schedule for this date, or <see langword="null"/> for a Holiday.</summary>
    public DayScheduleItem? CustomSchedule { get; set; }

    /// <summary>Gets or sets whether this override has been cancelled.</summary>
    public bool IsCancelled { get; set; }
}

/// <summary>Client-side projection of the API's <c>WorkCalendarDto</c>.</summary>
public sealed class WorkCalendarItem
{
    /// <summary>Gets or sets the Work Calendar's identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the owning Project's identifier.</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Gets or sets the calendar's display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the calendar's Work Patterns.</summary>
    public List<WorkPatternItem> WorkPatterns { get; set; } = new();

    /// <summary>Gets or sets the calendar's Day Overrides.</summary>
    public List<DayOverrideItem> DayOverrides { get; set; } = new();
}

/// <summary>
/// Matches the API's standard error response shape (see
/// <c>ResultExtensions.ToProblemResult</c>):
/// <c>{ errorCode, title, message, correlationId, details }</c>.
/// </summary>
public sealed class WorkCalendarApiError
{
    /// <summary>Gets or sets the machine-readable error code (e.g. "WorkCalendar.NotFound").</summary>
    public string? ErrorCode { get; set; }

    /// <summary>Gets or sets the short error title.</summary>
    public string? Title { get; set; }

    /// <summary>Gets or sets the human-readable error message.</summary>
    public string? Message { get; set; }
}
