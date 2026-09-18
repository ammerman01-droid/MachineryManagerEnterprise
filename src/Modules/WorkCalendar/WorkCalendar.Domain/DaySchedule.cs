using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain;

/// <summary>
/// Value Object representing the working schedule for a single day —
/// either a set of <see cref="Shift"/>s (a working day), or none (a day
/// off). Used both as a template for a day-of-week within a
/// <see cref="WorkPattern"/>, and as the resolved schedule for a
/// specific calendar date via <see cref="Domain.WorkCalendar.ResolveDay"/>.
/// </summary>
public sealed class DaySchedule
{
    private readonly List<Shift> _shifts;

    /// <summary>Gets the shifts worked on this day. Empty for a non-working day.</summary>
    public IReadOnlyList<Shift> Shifts => _shifts.AsReadOnly();

    /// <summary>Gets whether this day has at least one shift (i.e. is a working day).</summary>
    public bool IsWorkingDay => _shifts.Count > 0;

    /// <summary>Gets the total net working hours across all shifts for this day.</summary>
    public TimeSpan TotalNetHours => _shifts.Aggregate(TimeSpan.Zero, (sum, s) => sum + s.NetDuration);

    private DaySchedule(List<Shift> shifts)
    {
        _shifts = shifts;
    }

    /// <summary>Creates a <see cref="DaySchedule"/> representing a day off (no shifts).</summary>
    public static DaySchedule DayOff() => new([]);

    /// <summary>Creates a <see cref="DaySchedule"/> from one or more shifts.</summary>
    /// <param name="shifts">The shifts worked on this day. Must not overlap one another.</param>
    /// <returns>A <see cref="Result{DaySchedule}"/> containing the new value, or a validation error.</returns>
    public static Result<DaySchedule> Create(IReadOnlyList<Shift> shifts)
    {
        if (shifts.Count == 0)
        {
            return Result.Failure<DaySchedule>(WorkCalendarErrors.DayScheduleRequiresAtLeastOneShift());
        }

        var ordered = shifts.OrderBy(s => s.StartTime).ToList();

        for (var i = 1; i < ordered.Count; i++)
        {
            if (ordered[i].StartTime < ordered[i - 1].EndTime)
            {
                return Result.Failure<DaySchedule>(WorkCalendarErrors.OverlappingShifts());
            }
        }

        return new DaySchedule(ordered);
    }
}