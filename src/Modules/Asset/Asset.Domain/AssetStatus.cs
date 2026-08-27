namespace Asset.Domain;

/// <summary>
/// The lifecycle state of an Asset (Section 4.9, State Machines):
/// Draft → Registered → Commissioned → Operational ↔ Inactive → Retired → Disposed.
/// </summary>
public enum AssetStatus
{
    /// <summary>Not yet used by this first increment (single-step registration creates directly at Registered — chat, 2026-08-27); reserved for a future two-step registration flow.</summary>
    Draft = 0,

    /// <summary>Identity captured; not yet commissioned for use.</summary>
    Registered = 1,

    /// <summary>Commissioning complete; not yet placed into operation.</summary>
    Commissioned = 2,

    /// <summary>Actively in use.</summary>
    Operational = 3,

    /// <summary>Temporarily out of use; can return to Operational.</summary>
    Inactive = 4,

    /// <summary>Permanently withdrawn from use.</summary>
    Retired = 5,

    /// <summary>Final state — physically disposed of.</summary>
    Disposed = 6,
}