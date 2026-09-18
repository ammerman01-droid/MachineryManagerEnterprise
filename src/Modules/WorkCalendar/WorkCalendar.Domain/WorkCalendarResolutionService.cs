namespace WorkCalendar.Domain;

/// <summary>
/// Domain Service providing the named entry point for "resolve DaySchedule
/// for a DateOnly = Pattern+Override" (10.17/4.6). The actual resolution
/// logic lives on the aggregate itself (<see cref="Domain.WorkCalendar.ResolveDay"/>)
/// since it only needs the aggregate's own public state — this service
/// exists so other modules/consumers have a stable, named seam to depend
/// on instead of the aggregate's method directly (mirrors how
/// <see cref="WorkCalendarCapacityService"/> wraps range resolution).
/// </summary>
public static class WorkCalendarResolutionService
{
    /// <summary>Resolves the effective schedule for a single calendar date.</summary>
    /// <param name="calendar">The Work Calendar to resolve against.</param>
    /// <param name="date">The calendar date to resolve.</param>
    /// <returns>The resolved schedule for <paramref name="date"/>.</returns>
    public static DayResolution Resolve(Domain.WorkCalendar calendar, DateOnly date) =>
        calendar.ResolveDay(date);
}
