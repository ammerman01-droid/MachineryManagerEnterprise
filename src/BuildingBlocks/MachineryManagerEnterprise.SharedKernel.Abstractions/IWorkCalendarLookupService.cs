namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup into the WorkCalendar module,
/// allowing other modules to resolve the effective work schedule
/// for a Project without depending on WorkCalendar.Domain or
/// WorkCalendar.Application directly.
/// </summary>
public interface IWorkCalendarLookupService
{
    /// <summary>
    /// Resolves the effective work schedule for the specified Project
    /// and calendar date, or <see langword="null"/> when the Project
    /// has no Work Calendar.
    /// </summary>
    /// <param name="projectId">The Project whose Work Calendar should be resolved.</param>
    /// <param name="date">The calendar date to resolve.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    /// The resolved day schedule, or <see langword="null"/> when no
    /// Work Calendar exists for the Project.
    /// </returns>
    Task<WorkCalendarDayScheduleSnapshot?> ResolveDayScheduleAsync(
        Guid projectId,
        DateOnly date,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Read-only cross-module snapshot of the effective schedule for one
/// Work Calendar date.
/// </summary>
/// <param name="IsWorkingDay">Indicates whether the resolved date is a working day.</param>
/// <param name="Shifts">The shifts scheduled for the resolved date.</param>
public sealed record WorkCalendarDayScheduleSnapshot(
    bool IsWorkingDay,
    IReadOnlyList<ShiftWindow> Shifts);

/// <summary>
/// Read-only cross-module representation of a Work Calendar shift.
/// <see cref="Index"/> is a stable identifier for this shift within
/// its day (chat, 2026-09-29 — E7) — the Usage module needs it to
/// address a specific shift's reading across time, since
/// <see cref="StartTime"/>/<see cref="EndTime"/> alone are not a
/// stable key (a shift's scheduled times can be edited later without
/// changing which shift it is).
/// </summary>
/// <param name="Index">The shift's stable position within its day (0-based, per the day's own shift list).</param>
/// <param name="StartTime">The scheduled shift start time.</param>
/// <param name="EndTime">The scheduled shift end time. If earlier than <see cref="StartTime"/>, the shift crosses midnight and belongs, in its entirety, to the day it starts on.</param>
public sealed record ShiftWindow(
    int Index,
    TimeOnly StartTime,
    TimeOnly EndTime);
