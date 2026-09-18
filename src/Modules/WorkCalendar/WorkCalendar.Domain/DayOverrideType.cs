namespace WorkCalendar.Domain;

/// <summary>Categorizes why a specific calendar date deviates from its Work Pattern's normal schedule.</summary>
public enum DayOverrideType
{
    /// <summary>A normally-working day is instead a day off (e.g. a public holiday).</summary>
    Holiday = 0,

    /// <summary>A normally-non-working day is instead worked (e.g. a weekend called in for a deadline).</summary>
    Working = 1,

    /// <summary>A normally-working day has different hours than its Work Pattern specifies.</summary>
    Modified = 2,
}