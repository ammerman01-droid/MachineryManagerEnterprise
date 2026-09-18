using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain;

/// <summary>
/// Value Object representing a single continuous period of work within
/// a day (e.g. "08:00–17:00 with a 12:00–13:00 lunch break"). A
/// <see cref="DaySchedule"/> may contain more than one Shift (e.g. a
/// split shift).
/// </summary>
public sealed class Shift
{
    private readonly List<Break> _breaks;

    /// <summary>Gets the time the shift starts.</summary>
    public TimeOnly StartTime { get; }

    /// <summary>Gets the time the shift ends.</summary>
    public TimeOnly EndTime { get; }

    /// <summary>Gets the unpaid breaks within this shift.</summary>
    public IReadOnlyList<Break> Breaks => _breaks.AsReadOnly();

    /// <summary>Gets the net working duration of the shift (gross duration minus all breaks).</summary>
    public TimeSpan NetDuration => (EndTime - StartTime) - _breaks.Aggregate(TimeSpan.Zero, (sum, b) => sum + b.Duration);

    private Shift(TimeOnly startTime, TimeOnly endTime, List<Break> breaks)
    {
        StartTime = startTime;
        EndTime = endTime;
        _breaks = breaks;
    }

    /// <summary>Creates a new <see cref="Shift"/>.</summary>
    /// <param name="startTime">The time the shift starts.</param>
    /// <param name="endTime">The time the shift ends.</param>
    /// <param name="breaks">The unpaid breaks within the shift, each of which must fall entirely within [<paramref name="startTime"/>, <paramref name="endTime"/>] and must not overlap another break.</param>
    /// <returns>A <see cref="Result{Shift}"/> containing the new value, or a validation error.</returns>
    public static Result<Shift> Create(TimeOnly startTime, TimeOnly endTime, IReadOnlyList<Break>? breaks = null)
    {
        if (endTime <= startTime)
        {
            return Result.Failure<Shift>(WorkCalendarErrors.ShiftEndBeforeStart());
        }

        var breakList = (breaks ?? []).OrderBy(b => b.StartTime).ToList();

        foreach (var b in breakList)
        {
            if (b.StartTime < startTime || b.EndTime > endTime)
            {
                return Result.Failure<Shift>(WorkCalendarErrors.BreakOutsideShift());
            }
        }

        for (var i = 1; i < breakList.Count; i++)
        {
            if (breakList[i].StartTime < breakList[i - 1].EndTime)
            {
                return Result.Failure<Shift>(WorkCalendarErrors.OverlappingBreaks());
            }
        }

        return new Shift(startTime, endTime, breakList);
    }
}