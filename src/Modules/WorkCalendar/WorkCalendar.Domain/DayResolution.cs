namespace WorkCalendar.Domain;

/// <summary>
/// The result of resolving a specific calendar date against a
/// <see cref="Domain.WorkCalendar"/> — i.e. "what is the schedule for
/// this exact day, after accounting for both the underlying
/// <see cref="WorkPattern"/> and any <see cref="DayOverride"/>?" This
/// is a pure computation result, never persisted.
/// </summary>
/// <param name="Date">The calendar date this resolution is for.</param>
/// <param name="Schedule">The resolved schedule for this date.</param>
/// <param name="Source">Whether the resolved schedule came from a Day Override or the underlying Work Pattern.</param>
/// <param name="OverrideType">The Day Override's type, if <paramref name="Source"/> is <see cref="DayResolutionSource.Override"/>.</param>
public sealed record DayResolution(
    DateOnly Date,
    DaySchedule Schedule,
    DayResolutionSource Source,
    DayOverrideType? OverrideType);

/// <summary>Identifies which mechanism produced a <see cref="DayResolution"/>.</summary>
public enum DayResolutionSource
{
    /// <summary>The schedule came from the day-of-week template in the active Work Pattern.</summary>
    Pattern = 0,

    /// <summary>The schedule came from an explicit Day Override for this exact date.</summary>
    Override = 1,

    /// <summary>No active Work Pattern covers this date, and there is no Day Override for it.</summary>
    Unresolved = 2,
}