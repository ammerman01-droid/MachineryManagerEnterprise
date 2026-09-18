namespace WorkCalendar.Domain;

/// <summary>
/// Domain Service exposing read-only, non-mutating pre-checks (overlap /
/// duplicate-Override / shift-time-consistency) so callers — a UI form,
/// or <c>WorkCalendarApplicationService</c> — can validate a prospective
/// change BEFORE submitting the mutating Command. This is distinct from
/// the aggregate's own invariant checks (<see cref="Domain.WorkCalendar.AddWorkPattern"/>,
/// <see cref="WorkPattern.Create"/>, <see cref="Shift.Create"/>, etc.),
/// which remain the sole authority actually enforced on every mutation —
/// this service never bypasses them, it only lets a caller ask the same
/// question without committing to a write.
/// </summary>
public static class WorkCalendarValidationService
{
    /// <summary>
    /// Determines whether a Work Pattern with the given date range would
    /// overlap any existing pattern on this calendar (BR-018-003).
    /// </summary>
    /// <param name="calendar">The Work Calendar to check against.</param>
    /// <param name="startDate">The prospective pattern's start date.</param>
    /// <param name="endDate">The prospective pattern's end date.</param>
    /// <param name="excludingPatternId">
    /// A pattern to exclude from the check — used when validating a
    /// Supersede, where the pattern being replaced is expected to
    /// occupy the same window.
    /// </param>
    /// <returns><see langword="true"/> if the range would overlap an existing pattern.</returns>
    public static bool WouldOverlapExistingPattern(
        Domain.WorkCalendar calendar, DateOnly startDate, DateOnly endDate, WorkPatternId? excludingPatternId = null) =>
        calendar.WorkPatterns
            .Where(p => excludingPatternId is null || p.Id != excludingPatternId)
            .Any(p => p.StartDate <= endDate && startDate <= p.EndDate);

    /// <summary>
    /// Determines whether a non-cancelled Day Override already exists
    /// for the given date (BR-018: at most one active override per date).
    /// </summary>
    /// <param name="calendar">The Work Calendar to check against.</param>
    /// <param name="date">The date to check.</param>
    /// <returns><see langword="true"/> if an active override already exists for <paramref name="date"/>.</returns>
    public static bool HasConflictingDayOverride(Domain.WorkCalendar calendar, DateOnly date) =>
        calendar.DayOverrides.Any(o => !o.IsCancelled && o.Date == date);

    /// <summary>
    /// Determines whether every day of the week is present in the given
    /// weekly schedule (BR-018-004), without constructing a
    /// <see cref="WorkPattern"/>.
    /// </summary>
    /// <param name="weeklySchedule">The candidate weekly schedule.</param>
    /// <returns><see langword="true"/> if all seven days of the week are present.</returns>
    public static bool WeeklyScheduleIsComplete(IReadOnlyDictionary<DayOfWeek, DaySchedule> weeklySchedule) =>
        Enum.GetValues<DayOfWeek>().All(weeklySchedule.ContainsKey);
}
