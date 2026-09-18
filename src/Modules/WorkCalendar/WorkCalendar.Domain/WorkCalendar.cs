using WorkCalendar.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace WorkCalendar.Domain;

/// <summary>
/// Aggregate Root representing the full work-scheduling configuration
/// for a Project: a sequence of date-ranged <see cref="WorkPattern"/>
/// templates, plus explicit <see cref="DayOverride"/> exceptions for
/// individual dates. <see cref="ResolveDay"/> is the single entry point
/// for "what is the schedule on date X?", combining both.
/// </summary>
/// <remarks>
/// Redesign (chat, 2026-09-16): a Project has exactly one Work
/// Calendar, created lazily and never shown by name to the user, so
/// the calendar itself no longer has a lifecycle (the previous
/// Draft/Active/Archived <c>Status</c> is removed entirely). Only the
/// WorkPatterns within it have a lifecycle now, and that lifecycle is
/// driven by dates rather than manual Activate/Archive actions:
/// whichever pattern's date range covers "today" is authoritative;
/// once a pattern's range is fully in the past (or the user manually
/// retires it), it becomes Historical and a new pattern is free to
/// occupy the same date range.
/// </remarks>
public sealed class WorkCalendar : AggregateRoot<WorkCalendarId>
{
    /// <summary>The maximum allowed length for the calendar's name.</summary>
    public const int MaxNameLength = 200;

    private readonly List<WorkPattern> _workPatterns = [];
    private readonly List<DayOverride> _dayOverrides = [];

    /// <summary>Gets the identifier of the Project this calendar governs.</summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Gets the calendar's stored name (auto-derived from the Project's
    /// name at registration time). Presentation-only bookkeeping —
    /// never shown to the user, since a Project has exactly one
    /// calendar and naming it serves no purpose (chat, 2026-09-16).
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the Work Patterns that make up this calendar.</summary>
    public IReadOnlyList<WorkPattern> WorkPatterns => _workPatterns.AsReadOnly();

    /// <summary>Gets the explicit Day Overrides for this calendar.</summary>
    public IReadOnlyList<DayOverride> DayOverrides => _dayOverrides.AsReadOnly();

    // Reserved for ORM materialization only. Never used by application code.
    private WorkCalendar()
    {
    }

    private WorkCalendar(WorkCalendarId id, Guid projectId, string name)
        : base(id)
    {
        ProjectId = projectId;
        Name = name;
    }

    /// <summary>
    /// Registers a new Work Calendar for a Project. Existence of the
    /// referenced Project is validated by the caller (Application
    /// layer), not here, since the aggregate cannot check other
    /// modules' data.
    /// </summary>
    /// <param name="projectId">The Project this calendar will govern.</param>
    /// <param name="name">The display name (required, max 200 characters) — the Project's own name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{WorkCalendar}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<WorkCalendar> Register(Guid projectId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<WorkCalendar>(WorkCalendarErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<WorkCalendar>(WorkCalendarErrors.NameTooLong(MaxNameLength));
        }

        var calendar = new WorkCalendar(WorkCalendarId.New(), projectId, name.Trim());

        calendar.RaiseDomainEvent(new WorkCalendarRegistered(calendar.Id, projectId, calendar.Name, dateTimeProvider.UtcNow));

        return calendar;
    }

    /// <summary>
    /// Adds a new Work Pattern to this calendar. The new pattern's date
    /// range must not overlap any Draft or Active pattern — Superseded
    /// and Historical patterns no longer govern anything, so a new
    /// pattern may freely reuse their dates (chat, 2026-09-16).
    /// </summary>
    /// <param name="startDate">The first date this pattern applies to (inclusive).</param>
    /// <param name="endDate">The last date this pattern applies to (inclusive).</param>
    /// <param name="weeklySchedule">The template schedule for every day of the week — all seven days must be present.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{WorkPatternId}"/> containing the new pattern's identifier, or a validation error.</returns>
    public Result<WorkPatternId> AddWorkPattern(
        DateOnly startDate,
        DateOnly endDate,
        IReadOnlyDictionary<DayOfWeek, DaySchedule> weeklySchedule,
        IDateTimeProvider dateTimeProvider)
    {
        if (_workPatterns.Any(p => IsCurrentlyGoverning(p) && p.OverlapsWith(startDate, endDate)))
        {
            return Result.Failure<WorkPatternId>(WorkCalendarErrors.WorkPatternOverlap());
        }

        var result = WorkPattern.Create(startDate, endDate, weeklySchedule);

        if (result.IsFailure)
        {
            return Result.Failure<WorkPatternId>(result.Error);
        }

        _workPatterns.Add(result.Value);

        RaiseDomainEvent(new WorkPatternAdded(Id, result.Value.Id, startDate, endDate, dateTimeProvider.UtcNow));

        return Result.Success(result.Value.Id);
    }

    /// <summary>Transitions a Draft Work Pattern to Active.</summary>
    /// <param name="workPatternId">The identifier of the pattern to activate.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a business error.</returns>
    public Result ActivateWorkPattern(WorkPatternId workPatternId)
    {
        var pattern = _workPatterns.FirstOrDefault(p => p.Id == workPatternId);

        if (pattern is null)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotFound(workPatternId.Value));
        }

        return pattern.Activate();
    }

    /// <summary>Removes a Work Pattern from this calendar. Only patterns still in Draft may be removed.</summary>
    /// <param name="workPatternId">The identifier of the pattern to remove.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a business error.</returns>
    public Result RemoveWorkPattern(WorkPatternId workPatternId)
    {
        var pattern = _workPatterns.FirstOrDefault(p => p.Id == workPatternId);

        if (pattern is null)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotFound(workPatternId.Value));
        }

        if (pattern.Status != WorkPatternStatus.Draft)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotRemovable());
        }

        _workPatterns.Remove(pattern);

        return Result.Success();
    }

    /// <summary>
    /// Manually retires an Active Work Pattern directly to Historical —
    /// e.g. the user decides it no longer applies, with no replacement
    /// pattern being added (unlike <see cref="SupersedeWorkPattern"/>,
    /// which always adds a replacement in the same operation). Added
    /// (chat, 2026-09-16) per the user's explicit requirement that a
    /// pattern can be retired on demand, at any time, freeing its date
    /// range for a new pattern to reuse.
    /// </summary>
    /// <param name="workPatternId">The identifier of the Active pattern to retire.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a business error.</returns>
    public Result RetireWorkPattern(WorkPatternId workPatternId, IDateTimeProvider dateTimeProvider)
    {
        var pattern = _workPatterns.FirstOrDefault(p => p.Id == workPatternId);

        if (pattern is null)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotFound(workPatternId.Value));
        }

        var result = pattern.Retire();

        if (result.IsFailure)
        {
            return result;
        }

        RaiseDomainEvent(new WorkPatternRetired(Id, workPatternId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Automatically expires any Active or Superseded pattern whose
    /// entire date range has passed, moving it directly to Historical.
    /// Since this project has no background job runner, every
    /// Command/Query handler that loads this aggregate calls this
    /// first (chat, 2026-09-16) — a "read-repair" pattern: whichever
    /// operation happens to load the calendar next brings its
    /// lifecycle state up to date and persists the correction.
    /// </summary>
    /// <param name="today">Today's date, from <see cref="IDateTimeProvider"/>.</param>
    /// <returns><see langword="true"/> if any pattern's status changed.</returns>
    public bool SynchronizeLifecycle(DateOnly today)
    {
        var changed = false;

        foreach (var pattern in _workPatterns)
        {
            if (pattern.ActivateIfDue(today) || pattern.ExpireIfPast(today))
            {
                changed = true;
            }
        }

        return changed;
    }

    /// <summary>
    /// Adds an explicit Day Override for a specific date. At most one
    /// non-cancelled override may exist per date.
    /// </summary>
    /// <param name="date">The calendar date this override applies to.</param>
    /// <param name="type">The category of this override.</param>
    /// <param name="customSchedule">The schedule for this date — required unless <paramref name="type"/> is <see cref="DayOverrideType.Holiday"/>.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{DayOverrideId}"/> containing the new override's identifier, or a validation error.</returns>
    public Result<DayOverrideId> AddDayOverride(
        DateOnly date, DayOverrideType type, DaySchedule? customSchedule, IDateTimeProvider dateTimeProvider)
    {
        if (_dayOverrides.Any(o => !o.IsCancelled && o.Date == date))
        {
            return Result.Failure<DayOverrideId>(WorkCalendarErrors.DayOverrideConflict());
        }

        var result = DayOverride.Create(date, type, customSchedule);

        if (result.IsFailure)
        {
            return Result.Failure<DayOverrideId>(result.Error);
        }

        _dayOverrides.Add(result.Value);

        RaiseDomainEvent(new DayOverrideAdded(Id, result.Value.Id, date, type, dateTimeProvider.UtcNow));

        return Result.Success(result.Value.Id);
    }

    /// <summary>Cancels a Day Override (retained for the audit trail, never deleted).</summary>
    /// <param name="dayOverrideId">The identifier of the override to cancel.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a business error.</returns>
    public Result CancelDayOverride(DayOverrideId dayOverrideId, IDateTimeProvider dateTimeProvider)
    {
        var dayOverride = _dayOverrides.FirstOrDefault(o => o.Id == dayOverrideId);

        if (dayOverride is null)
        {
            return Result.Failure(WorkCalendarErrors.DayOverrideNotFound(dayOverrideId.Value));
        }

        var result = dayOverride.Cancel();

        if (result.IsFailure)
        {
            return result;
        }

        RaiseDomainEvent(new DayOverrideCancelled(Id, dayOverrideId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Resolves the effective schedule for a specific calendar date, by
    /// checking (in order of precedence): (1) a non-cancelled Day
    /// Override for this exact date, then (2) the Active Work Pattern
    /// whose date range covers this date, then (3) — only if no Active
    /// pattern covers it — the most recent Superseded/Historical
    /// pattern that does (chat, 2026-09-16: needed once overlapping a
    /// retired pattern's dates became allowed, so a live Active pattern
    /// always wins over an old one nominally covering the same dates).
    /// If nothing applies, the date is unresolved.
    /// </summary>
    /// <param name="date">The calendar date to resolve.</param>
    /// <returns>The resolved schedule for <paramref name="date"/>.</returns>
    public DayResolution ResolveDay(DateOnly date)
    {
        var activeOverride = _dayOverrides.FirstOrDefault(o => !o.IsCancelled && o.Date == date);

        if (activeOverride is not null)
        {
            var schedule = activeOverride.CustomSchedule ?? DaySchedule.DayOff();
            return new DayResolution(date, schedule, DayResolutionSource.Override, activeOverride.Type);
        }

        var activePattern = _workPatterns.FirstOrDefault(p => p.Status == WorkPatternStatus.Active && p.Covers(date));

        var governingPattern = activePattern ?? _workPatterns
            .Where(p => (p.Status == WorkPatternStatus.Superseded || p.Status == WorkPatternStatus.Historical) && p.Covers(date))
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefault();

        if (governingPattern is not null)
        {
            var schedule = governingPattern.WeeklySchedule[date.DayOfWeek];
            return new DayResolution(date, schedule, DayResolutionSource.Pattern, null);
        }

        return new DayResolution(date, DaySchedule.DayOff(), DayResolutionSource.Unresolved, null);
    }

    /// <summary>
    /// Replaces an existing Active Work Pattern with a new version
    /// (BR-018-010: editing a Pattern never mutates it in place). The
    /// old pattern transitions to Superseded (retained for historical
    /// date-resolution, never deleted); the new pattern starts directly
    /// in Active status so the calendar has no gap in coverage.
    /// </summary>
    /// <param name="patternToSupersedeId">The identifier of the Active pattern being replaced.</param>
    /// <param name="startDate">The first date the new pattern version applies to (inclusive).</param>
    /// <param name="endDate">The last date the new pattern version applies to (inclusive).</param>
    /// <param name="weeklySchedule">The new version's template schedule for every day of the week — all seven days must be present.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{WorkPatternId}"/> containing the new version's identifier, or a business error.</returns>
    public Result<WorkPatternId> SupersedeWorkPattern(
        WorkPatternId patternToSupersedeId,
        DateOnly startDate,
        DateOnly endDate,
        IReadOnlyDictionary<DayOfWeek, DaySchedule> weeklySchedule,
        IDateTimeProvider dateTimeProvider)
    {
        var patternToSupersede = _workPatterns.FirstOrDefault(p => p.Id == patternToSupersedeId);

        if (patternToSupersede is null)
        {
            return Result.Failure<WorkPatternId>(WorkCalendarErrors.WorkPatternNotFound(patternToSupersedeId.Value));
        }

        // Overlap is checked against every OTHER currently-governing
        // pattern — the one being superseded is expected to occupy the
        // same window and is excluded; Superseded/Historical patterns
        // no longer govern anything, so they never block this either.
        if (_workPatterns.Any(p => p.Id != patternToSupersedeId && IsCurrentlyGoverning(p) && p.OverlapsWith(startDate, endDate)))
        {
            return Result.Failure<WorkPatternId>(WorkCalendarErrors.WorkPatternOverlap());
        }

        var newPatternResult = WorkPattern.CreateActive(startDate, endDate, weeklySchedule);

        if (newPatternResult.IsFailure)
        {
            return Result.Failure<WorkPatternId>(newPatternResult.Error);
        }

        var supersedeResult = patternToSupersede.Supersede();

        if (supersedeResult.IsFailure)
        {
            return Result.Failure<WorkPatternId>(supersedeResult.Error);
        }

        _workPatterns.Add(newPatternResult.Value);

        RaiseDomainEvent(new WorkPatternSuperseded(
            Id, patternToSupersedeId, newPatternResult.Value.Id, dateTimeProvider.UtcNow));

        return Result.Success(newPatternResult.Value.Id);
    }

    /// <summary>
    /// Marks a Superseded Work Pattern as Historical, once its entire
    /// date range is confirmed to be in the past. Superseded by
    /// <see cref="SynchronizeLifecycle"/> for normal operation
    /// (chat, 2026-09-16) — kept for any direct/manual use.
    /// </summary>
    /// <param name="workPatternId">The identifier of the Superseded pattern to mark Historical.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a business error.</returns>
    public Result MarkWorkPatternHistorical(WorkPatternId workPatternId)
    {
        var pattern = _workPatterns.FirstOrDefault(p => p.Id == workPatternId);

        if (pattern is null)
        {
            return Result.Failure(WorkCalendarErrors.WorkPatternNotFound(workPatternId.Value));
        }

        return pattern.MarkHistorical();
    }

    /// <summary>A pattern currently governs day resolution and blocks overlaps if it is Draft or Active.</summary>
    private static bool IsCurrentlyGoverning(WorkPattern pattern) =>
        pattern.Status is WorkPatternStatus.Draft or WorkPatternStatus.Active;
}