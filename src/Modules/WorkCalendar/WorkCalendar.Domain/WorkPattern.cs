using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace WorkCalendar.Domain;

/// <summary>
/// Entity representing a named weekly schedule template that applies
/// for a specific date range within a <see cref="Domain.WorkCalendar"/>
/// (e.g. "Winter schedule, 2026-10-01 to 2027-03-20"). Owned by
/// <see cref="Domain.WorkCalendar"/> — never created, modified, or
/// queried independently of its owning aggregate.
/// </summary>
public sealed class WorkPattern : Entity<WorkPatternId>
{
    /// <summary>Gets the first date this pattern applies to (inclusive).</summary>
    public DateOnly StartDate { get; private set; }

    /// <summary>Gets the last date this pattern applies to (inclusive).</summary>
    public DateOnly EndDate { get; private set; }

    /// <summary>Gets the current lifecycle status of this pattern.</summary>
    public WorkPatternStatus Status { get; private set; }

    private readonly Dictionary<DayOfWeek, DaySchedule> _weeklySchedule;

    /// <summary>Gets the template schedule for each day of the week.</summary>
    public IReadOnlyDictionary<DayOfWeek, DaySchedule> WeeklySchedule => _weeklySchedule;

    // Reserved for ORM materialization only. Never used by application code.
    private WorkPattern()
    {
        _weeklySchedule = new Dictionary<DayOfWeek, DaySchedule>();
    }

    private WorkPattern(
        WorkPatternId id, DateOnly startDate, DateOnly endDate, Dictionary<DayOfWeek, DaySchedule> weeklySchedule)
        : base(id)
    {
        StartDate = startDate;
        EndDate = endDate;
        Status = WorkPatternStatus.Draft;
        _weeklySchedule = weeklySchedule;
    }

    /// <summary>
    /// Creates a new <see cref="WorkPattern"/>. Always starts as
    /// <see cref="WorkPatternStatus.Draft"/> — overlap with other
    /// patterns in the same <see cref="Domain.WorkCalendar"/> is
    /// checked by the aggregate, not here, since that requires
    /// knowledge of sibling entities.
    /// </summary>
    /// <param name="startDate">The first date this pattern applies to (inclusive).</param>
    /// <param name="endDate">The last date this pattern applies to (inclusive).</param>
    /// <param name="weeklySchedule">The template schedule for every day of the week — all seven days must be present.</param>
    /// <returns>A <see cref="Result{WorkPattern}"/> containing the new entity, or a validation error.</returns>
    internal static Result<WorkPattern> Create(
        DateOnly startDate, DateOnly endDate, IReadOnlyDictionary<DayOfWeek, DaySchedule> weeklySchedule)
    {
        if (endDate < startDate)
        {
            return Result.Failure<WorkPattern>(WorkCalendarErrors.WorkPatternEndBeforeStart());
        }

        var allDaysOfWeek = Enum.GetValues<DayOfWeek>();

        if (allDaysOfWeek.Any(day => !weeklySchedule.ContainsKey(day)))
        {
            return Result.Failure<WorkPattern>(WorkCalendarErrors.WorkPatternMissingDayOfWeek());
        }

        return new WorkPattern(WorkPatternId.New(), startDate, endDate, new Dictionary<DayOfWeek, DaySchedule>(weeklySchedule));
    }

    /// <summary>
    /// Creates a new <see cref="WorkPattern"/> that starts directly in
    /// <see cref="WorkPatternStatus.Active"/> — used only by
    /// <see cref="Domain.WorkCalendar.SupersedeWorkPattern"/>, where the
    /// replacement must take over immediately (otherwise the calendar
    /// would have a gap with no Active pattern covering the transition
    /// period).
    /// </summary>
    /// <param name="startDate">The first date this pattern applies to (inclusive).</param>
    /// <param name="endDate">The last date this pattern applies to (inclusive).</param>
    /// <param name="weeklySchedule">The template schedule for every day of the week — all seven days must be present.</param>
    /// <returns>A <see cref="Result{WorkPattern}"/> containing the new, already-Active entity, or a validation error.</returns>
    internal static Result<WorkPattern> CreateActive(
        DateOnly startDate, DateOnly endDate, IReadOnlyDictionary<DayOfWeek, DaySchedule> weeklySchedule)
    {
        var result = Create(startDate, endDate, weeklySchedule);

        if (result.IsFailure)
        {
            return result;
        }

        result.Value.Status = WorkPatternStatus.Active;

        return result;
    }

    /// <summary>Determines whether this pattern's date range overlaps the given range.</summary>
    /// <param name="otherStartDate">The other range's start date.</param>
    /// <param name="otherEndDate">The other range's end date.</param>
    /// <returns><see langword="true"/> if the two ranges share at least one date; otherwise <see langword="false"/>.</returns>
    internal bool OverlapsWith(DateOnly otherStartDate, DateOnly otherEndDate) =>
        StartDate <= otherEndDate && otherStartDate <= EndDate;

    /// <summary>
    /// Determines whether the given date falls within this pattern's date range.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="date"/> is within
    /// [<see cref="StartDate"/>, <see cref="EndDate"/>];
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Covers(DateOnly date) => date >= StartDate && date <= EndDate;

    /// <summary>Transitions this pattern from Draft to Active.</summary>
    /// <returns>A <see cref="Result"/> indicating success, or a conflict error if not currently Draft.</returns>
    internal Result Activate()
    {
        if (Status != WorkPatternStatus.Draft)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotDraft());
        }

        Status = WorkPatternStatus.Active;

        return Result.Success();
    }

    /// <summary>
    /// Transitions this pattern from Active to Superseded (BR-018-010).
    /// Only an Active pattern may be superseded — a Draft pattern that
    /// no longer applies should be removed instead (see
    /// <see cref="Domain.WorkCalendar.RemoveWorkPattern"/>), and a
    /// pattern cannot skip directly from Draft to Superseded.
    /// </summary>
    /// <returns>A <see cref="Result"/> indicating success, or a conflict error if not currently Active.</returns>
    internal Result Supersede()
    {
        if (Status != WorkPatternStatus.Active)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotActive());
        }

        Status = WorkPatternStatus.Superseded;

        return Result.Success();
    }

    /// <summary>
    /// Transitions this pattern from Superseded to Historical — its
    /// entire date range is now in the past and it is retained purely
    /// for audit purposes.
    /// </summary>
    /// <returns>A <see cref="Result"/> indicating success, or a conflict error if not currently Superseded.</returns>
    internal Result MarkHistorical()
    {
        if (Status != WorkPatternStatus.Superseded)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotSuperseded());
        }

        Status = WorkPatternStatus.Historical;

        return Result.Success();
    }

    /// <summary>
    /// Manually retires this pattern from Active directly to Historical
    /// — no replacement is being added in the same operation (that's
    /// what distinguishes this from <see cref="Supersede"/>).
    /// </summary>
    /// <returns>A <see cref="Result"/> indicating success, or a conflict error if not currently Active.</returns>
    internal Result Retire()
    {
        if (Status != WorkPatternStatus.Active)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotRetirable());
        }

        Status = WorkPatternStatus.Historical;

        return Result.Success();
    }

    /// <summary>
    /// If this pattern is Active or Superseded and its entire date
    /// range has passed relative to <paramref name="today"/>, moves it
    /// directly to Historical. No-op otherwise.
    /// </summary>
    /// <param name="today">Today's date, from <see cref="IDateTimeProvider"/>.</param>
    /// <returns><see langword="true"/> if this pattern's status changed.</returns>
    internal bool ExpireIfPast(DateOnly today)
    {
        if (Status is not (WorkPatternStatus.Active or WorkPatternStatus.Superseded) || EndDate >= today)
        {
            return false;
        }

        Status = WorkPatternStatus.Historical;
        return true;
    }

    /// <summary>
    /// If this pattern is Draft and its start date has arrived, moves
    /// it to Active — the automatic counterpart to manual
    /// <see cref="Activate"/>, added (chat, 2026-09-17) to complete the
    /// date-driven lifecycle: a pattern becomes the one everyone uses
    /// the moment its window begins, with no manual click required.
    /// </summary>
    /// <param name="today">Today's date, from <see cref="IDateTimeProvider"/>.</param>
    /// <returns><see langword="true"/> if this pattern's status changed.</returns>
    internal bool ActivateIfDue(DateOnly today)
    {
        if (Status != WorkPatternStatus.Draft || StartDate > today)
        {
            return false;
        }

        Status = WorkPatternStatus.Active;
        return true;
    }
}