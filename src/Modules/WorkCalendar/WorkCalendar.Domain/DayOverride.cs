using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace WorkCalendar.Domain;

/// <summary>
/// Entity representing an explicit exception to the normal schedule for
/// one specific calendar date (e.g. a public holiday, or a weekend
/// worked for a deadline). Takes precedence over the underlying
/// <see cref="WorkPattern"/> for that date. Owned by
/// <see cref="Domain.WorkCalendar"/>.
/// </summary>
public sealed class DayOverride : Entity<DayOverrideId>
{
    /// <summary>Gets the calendar date this override applies to.</summary>
    public DateOnly Date { get; private set; }

    /// <summary>Gets the category of this override.</summary>
    public DayOverrideType Type { get; private set; }

    /// <summary>
    /// Gets the schedule to use for this date, or <see langword="null"/>
    /// for a <see cref="DayOverrideType.Holiday"/> (a day off has no
    /// schedule to define).
    /// </summary>
    public DaySchedule? CustomSchedule { get; private set; }

    /// <summary>
    /// Gets whether this override has been cancelled. A cancelled
    /// override is never deleted (audit trail) — it is simply ignored
    /// during day resolution.
    /// </summary>
    public bool IsCancelled { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private DayOverride()
    {
    }

    private DayOverride(DayOverrideId id, DateOnly date, DayOverrideType type, DaySchedule? customSchedule)
        : base(id)
    {
        Date = date;
        Type = type;
        CustomSchedule = customSchedule;
        IsCancelled = false;
    }

    /// <summary>
    /// Creates a new <see cref="DayOverride"/>. Conflict with an
    /// existing non-cancelled override for the same date is checked by
    /// the aggregate, not here, since that requires knowledge of
    /// sibling entities.
    /// </summary>
    /// <param name="date">The calendar date this override applies to.</param>
    /// <param name="type">The category of this override.</param>
    /// <param name="customSchedule">
    /// The schedule for this date. Required for
    /// <see cref="DayOverrideType.Working"/> and
    /// <see cref="DayOverrideType.Modified"/> (there must be a schedule
    /// to work); must be <see langword="null"/> for
    /// <see cref="DayOverrideType.Holiday"/> (a day off has no schedule).
    /// </param>
    /// <returns>A <see cref="Result{DayOverride}"/> containing the new entity, or a validation error.</returns>
    internal static Result<DayOverride> Create(DateOnly date, DayOverrideType type, DaySchedule? customSchedule)
    {
        if (type == DayOverrideType.Holiday && customSchedule is not null)
        {
            return Result.Failure<DayOverride>(WorkCalendarErrors.HolidayOverrideMustNotHaveSchedule());
        }

        if (type != DayOverrideType.Holiday && customSchedule is null)
        {
            return Result.Failure<DayOverride>(WorkCalendarErrors.NonHolidayOverrideRequiresSchedule());
        }

        return new DayOverride(DayOverrideId.New(), date, type, customSchedule);
    }

    /// <summary>Marks this override as cancelled. The record is retained, not removed (audit trail).</summary>
    /// <returns>A <see cref="Result"/> indicating success, or a conflict error if already cancelled.</returns>
    internal Result Cancel()
    {
        if (IsCancelled)
        {
            return Result.Failure(WorkCalendarErrors.DayOverrideAlreadyCancelled());
        }

        IsCancelled = true;

        return Result.Success();
    }
}