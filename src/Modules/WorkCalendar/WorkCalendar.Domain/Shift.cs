using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain;

/// <summary>
/// Value Object representing a single continuous period of work within
/// a day (e.g. "08:00–17:00 with a 12:00–13:00 lunch break"). A
/// <see cref="DaySchedule"/> may contain more than one Shift (e.g. a
/// split shift).
/// </summary>
/// <remarks>
/// A shift may cross midnight (an overnight / night shift, e.g.
/// 22:00–06:00): this is expressed by an <see cref="EndTime"/> earlier
/// than the <see cref="StartTime"/>. The shift always belongs, in its
/// entirety, to the day it STARTS on — for day resolution and capacity
/// calculation alike — so a Saturday 22:00–06:00 shift counts fully
/// under Saturday, and the state of Sunday (a holiday override, another
/// pattern, …) has no effect on it. A shift is limited to
/// <see cref="MaxDurationHours"/> hours, which also rules out a
/// 24-hour shift (start equal to end).
/// </remarks>
public sealed class Shift
{
    /// <summary>The maximum allowed gross duration of a single shift, in hours.</summary>
    public const int MaxDurationHours = 12;

    private readonly List<Break> _breaks;

    /// <summary>Gets the time the shift starts.</summary>
    public TimeOnly StartTime { get; }

    /// <summary>Gets the time the shift ends. If earlier than <see cref="StartTime"/>, the shift ends on the following day.</summary>
    public TimeOnly EndTime { get; }

    /// <summary>Gets the unpaid breaks within this shift, ordered by their position within the shift.</summary>
    public IReadOnlyList<Break> Breaks => _breaks.AsReadOnly();

    /// <summary>Gets whether the shift ends on the day after it starts (an overnight shift).</summary>
    public bool CrossesMidnight => EndTime < StartTime;

    /// <summary>Gets the gross duration of the shift (start to end, before subtracting breaks), taking a midnight crossing into account.</summary>
    public TimeSpan GrossDuration => Break.ComputeDuration(StartTime, EndTime);

    /// <summary>Gets the net working duration of the shift (gross duration minus all breaks).</summary>
    public TimeSpan NetDuration => GrossDuration - _breaks.Aggregate(TimeSpan.Zero, (sum, b) => sum + b.Duration);

    /// <summary>
    /// Gets the moment the shift ends, measured from midnight at the
    /// start of the day the shift begins on. Values above 24 hours mean
    /// the shift runs into the following day.
    /// </summary>
    public TimeSpan EndOffsetFromStartOfDay => StartTime.ToTimeSpan() + GrossDuration;

    /// <summary>Gets how far into the following day the shift runs (zero if it ends on the day it starts).</summary>
    public TimeSpan SpillOverDuration
    {
        get
        {
            var spill = EndOffsetFromStartOfDay - TimeSpan.FromDays(1);

            return spill > TimeSpan.Zero ? spill : TimeSpan.Zero;
        }
    }

    private Shift(TimeOnly startTime, TimeOnly endTime, List<Break> breaks)
    {
        StartTime = startTime;
        EndTime = endTime;
        _breaks = breaks;
    }

    /// <summary>Creates a new <see cref="Shift"/>.</summary>
    /// <param name="startTime">The time the shift starts.</param>
    /// <param name="endTime">The time the shift ends. If earlier than <paramref name="startTime"/>, the shift is an overnight shift ending on the following day. Must differ from <paramref name="startTime"/>.</param>
    /// <param name="breaks">The unpaid breaks within the shift, each of which must fall entirely within the shift's span (measured from its start, so a break after midnight is valid in an overnight shift) and must not overlap another break.</param>
    /// <returns>A <see cref="Result{Shift}"/> containing the new value, or a validation error.</returns>
    public static Result<Shift> Create(TimeOnly startTime, TimeOnly endTime, IReadOnlyList<Break>? breaks = null)
    {
        if (endTime == startTime)
        {
            return Result.Failure<Shift>(WorkCalendarErrors.ShiftStartEqualsEnd());
        }

        var grossDuration = Break.ComputeDuration(startTime, endTime);

        if (grossDuration > TimeSpan.FromHours(MaxDurationHours))
        {
            return Result.Failure<Shift>(WorkCalendarErrors.ShiftTooLong(MaxDurationHours));
        }

        // Each break is positioned by its offset from the shift's own
        // start, which makes containment and overlap checks identical
        // for ordinary and overnight shifts.
        var positioned = (breaks ?? [])
            .Select(b => (Break: b, Offset: Break.ComputeDuration(startTime, b.StartTime)))
            .OrderBy(x => x.Offset)
            .ToList();

        foreach (var (candidate, offset) in positioned)
        {
            if (offset + candidate.Duration > grossDuration)
            {
                return Result.Failure<Shift>(WorkCalendarErrors.BreakOutsideShift());
            }
        }

        for (var i = 1; i < positioned.Count; i++)
        {
            if (positioned[i].Offset < positioned[i - 1].Offset + positioned[i - 1].Break.Duration)
            {
                return Result.Failure<Shift>(WorkCalendarErrors.OverlappingBreaks());
            }
        }

        return new Shift(startTime, endTime, positioned.Select(x => x.Break).ToList());
    }
}
