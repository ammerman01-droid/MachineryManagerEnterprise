namespace Usage.Domain;

/// <summary>
/// Distinguishes how a <see cref="UsageEntry"/> came to exist (chat,
/// 2026-09-29).
/// </summary>
public enum UsageEntryOrigin
{
    /// <summary>Entered directly by a user.</summary>
    Manual = 0,

    /// <summary>
    /// Generated automatically to fill a mandatory shift the user did
    /// not report on, with zero Operational Usage (BR-044).
    /// </summary>
    AutoFilled = 1,
}
