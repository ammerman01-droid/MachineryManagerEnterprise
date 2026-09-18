namespace WorkCalendar.Domain;

/// <summary>
/// Domain Service that computes aggregate working-capacity information
/// over a date range for a Work Calendar — used by Maintenance-window
/// planning and utilization-denominator calculations elsewhere in the
/// platform. Extracted as a separate service (rather than a method on
/// <see cref="Domain.WorkCalendar"/> itself) because it operates over a
/// potentially large date range by repeatedly invoking the aggregate's
/// public <see cref="Domain.WorkCalendar.ResolveDay"/>, rather than
/// needing access to the aggregate's private state directly.
/// </summary>
public static class WorkCalendarCapacityService
{
    /// <summary>
    /// Resolves every date in [<paramref name="startDate"/>, <paramref name="endDate"/>]
    /// (inclusive) and returns the day-by-day resolution results.
    /// </summary>
    /// <param name="calendar">The Work Calendar to resolve against.</param>
    /// <param name="startDate">The first date in the range (inclusive).</param>
    /// <param name="endDate">The last date in the range (inclusive).</param>
    /// <returns>One <see cref="DayResolution"/> per date in the range, in date order.</returns>
    public static IReadOnlyList<DayResolution> ResolveRange(
        Domain.WorkCalendar calendar, DateOnly startDate, DateOnly endDate)
    {
        var results = new List<DayResolution>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            results.Add(calendar.ResolveDay(date));
        }

        return results;
    }

    /// <summary>
    /// Computes the total net working hours across every date in
    /// [<paramref name="startDate"/>, <paramref name="endDate"/>]
    /// (inclusive) — the "available hours" figure Maintenance-window
    /// planning and utilization calculations consume (BR-018-009).
    /// </summary>
    /// <param name="calendar">The Work Calendar to resolve against.</param>
    /// <param name="startDate">The first date in the range (inclusive).</param>
    /// <param name="endDate">The last date in the range (inclusive).</param>
    /// <returns>The total net working hours across the range.</returns>
    public static TimeSpan CalculateTotalCapacity(Domain.WorkCalendar calendar, DateOnly startDate, DateOnly endDate)
    {
        return ResolveRange(calendar, startDate, endDate)
            .Aggregate(TimeSpan.Zero, (sum, resolution) => sum + resolution.Schedule.TotalNetHours);
    }
}