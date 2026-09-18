namespace MachineryManagerEnterprise.WorkCalendar.Presentation.Contracts;

/// <summary>Request body for registering a new Work Calendar.</summary>
public sealed record RegisterWorkCalendarRequest(Guid ProjectId, string Name);

/// <summary>Wire shape for an unpaid break within a shift.</summary>
public sealed record BreakRequest(TimeOnly StartTime, TimeOnly EndTime);

/// <summary>Wire shape for a single shift within a day.</summary>
public sealed record ShiftRequest(TimeOnly StartTime, TimeOnly EndTime, IReadOnlyList<BreakRequest>? Breaks);

/// <summary>Wire shape for a day's schedule — omit or leave <see cref="Shifts"/> empty for a day off.</summary>
public sealed record DayScheduleRequest(IReadOnlyList<ShiftRequest>? Shifts);

/// <summary>Request body for adding a new Work Pattern.</summary>
public sealed record AddWorkPatternRequest(
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyDictionary<DayOfWeek, DayScheduleRequest> WeeklySchedule);

/// <summary>Request body for superseding an existing Active Work Pattern.</summary>
public sealed record SupersedeWorkPatternRequest(
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyDictionary<DayOfWeek, DayScheduleRequest> WeeklySchedule);

/// <summary>Request body for adding a Day Override.</summary>
public sealed record AddDayOverrideRequest(
    DateOnly Date,
    global::WorkCalendar.Domain.DayOverrideType Type,
    DayScheduleRequest? CustomSchedule);
