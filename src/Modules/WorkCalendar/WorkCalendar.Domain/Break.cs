using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain;

/// <summary>
/// Value Object representing a single unpaid break within a
/// <see cref="Shift"/> (e.g. a lunch break). Immutable — breaks are
/// never mutated in place, only replaced as part of rebuilding the
/// owning <see cref="Shift"/>.
/// </summary>
public sealed class Break
{
    /// <summary>Gets the time the break starts.</summary>
    public TimeOnly StartTime { get; }

    /// <summary>Gets the time the break ends.</summary>
    public TimeOnly EndTime { get; }

    /// <summary>Gets the duration of the break.</summary>
    public TimeSpan Duration => EndTime - StartTime;

    private Break(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>Creates a new <see cref="Break"/>.</summary>
    /// <param name="startTime">The time the break starts.</param>
    /// <param name="endTime">The time the break ends.</param>
    /// <returns>A <see cref="Result{Break}"/> containing the new value, or a validation error.</returns>
    public static Result<Break> Create(TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
        {
            return Result.Failure<Break>(WorkCalendarErrors.BreakEndBeforeStart());
        }

        return new Break(startTime, endTime);
    }
}