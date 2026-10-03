namespace Usage.Domain;

/// <summary>
/// Distinguishes the two kinds of entries that can appear on a
/// <see cref="UsageLedger"/>'s timeline (chat, 2026-09-29).
/// </summary>
public enum UsageEntryKind
{
    /// <summary>A normal reading tied to a specific Work Calendar shift.</summary>
    ShiftReading = 0,

    /// <summary>
    /// A counter rebase: the device was replaced or its counter reset,
    /// so this entry re-anchors the Ledger's baseline without itself
    /// counting toward Operational Usage.
    /// </summary>
    Rebase = 1,
}
