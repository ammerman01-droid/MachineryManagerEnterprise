using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain;

/// <summary>
/// Value Object representing a single unpaid break within a
/// <see cref="Shift"/> (e.g. a lunch break). Immutable — breaks are
/// never mutated in place, only replaced as part of rebuilding the
/// owning <see cref="Shift"/>.
/// </summary>
/// <remarks>
/// A break may cross midnight (its <see cref="EndTime"/> is earlier
/// than its <see cref="StartTime"/>, e.g. 23:30–00:30) — this is only
/// meaningful inside an overnight <see cref="Shift"/>, and the owning
/// shift rejects it if it does not fit within the shift's own span.
/// </remarks>
public sealed class Break
{
    /// <summary>Gets the time the break starts.</summary>
    public TimeOnly StartTime { get; }

    /// <summary>Gets the time the break ends.</summary>
    public TimeOnly EndTime { get; }

    /// <summary>Gets whether the break ends on the day after it starts (its end time is earlier than its start time).</summary>
    public bool CrossesMidnight => EndTime < StartTime;

    /// <summary>Gets the duration of the break, taking a midnight crossing into account.</summary>
    public TimeSpan Duration => ComputeDuration(StartTime, EndTime);

    private Break(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>Creates a new <see cref="Break"/>.</summary>
    /// <param name="startTime">The time the break starts.</param>
    /// <param name="endTime">The time the break ends. If earlier than <paramref name="startTime"/>, the break ends on the following day.</param>
    /// <returns>A <see cref="Result{Break}"/> containing the new value, or a validation error.</returns>
    public static Result<Break> Create(TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime == startTime)
        {
            return Result.Failure<Break>(WorkCalendarErrors.BreakStartEqualsEnd());
        }

        return new Break(startTime, endTime);
    }

    /// <summary>
    /// Computes the forward distance from <paramref name="start"/> to
    /// <paramref name="end"/> on a 24-hour clock: when <paramref name="end"/>
    /// is earlier than <paramref name="start"/>, the interval is taken
    /// to cross midnight. Shared by <see cref="Shift"/> so both types
    /// interpret a midnight crossing identically.
    /// </summary>
    /// <param name="start">The interval's start time.</param>
    /// <param name="end">The interval's end time.</param>
    /// <returns>The interval's length, in the range [0, 24 hours).</returns>
    internal static TimeSpan ComputeDuration(TimeOnly start, TimeOnly end)
    {
        var difference = end.ToTimeSpan() - start.ToTimeSpan();

        return difference < TimeSpan.Zero ? difference + TimeSpan.FromDays(1) : difference;
    }
}
