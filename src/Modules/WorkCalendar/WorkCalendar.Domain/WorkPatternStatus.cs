namespace WorkCalendar.Domain;

/// <summary>
/// The lifecycle status of a <see cref="Domain.WorkPattern"/> within a
/// Work Calendar (BR-018-010: editing a Pattern never mutates it in
/// place — it creates a new version and moves the old one to
/// Superseded, preserving it for historical date-resolution).
/// </summary>
public enum WorkPatternStatus
{
    /// <summary>Newly added; may still be freely removed. Does not yet affect day resolution.</summary>
    Draft = 0,

    /// <summary>In effect for its date range; governs day resolution for any date it covers.</summary>
    Active = 1,

    /// <summary>
    /// Replaced by a newer version via <see cref="Domain.WorkCalendar.SupersedeWorkPattern"/>.
    /// No longer governs day resolution, but is retained (never
    /// deleted) so that dates it used to cover can still be resolved
    /// historically as of that point in time.
    /// </summary>
    Superseded = 2,

    /// <summary>
    /// A Superseded pattern whose entire date range is now in the past
    /// and is retained purely for audit/history purposes.
    /// </summary>
    Historical = 3,
}