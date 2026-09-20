namespace Asset.Domain;

/// <summary>
/// The status of an Asset within the fleet (chat, 2026-09-20). Replaces
/// the earlier Draft → Registered → Commissioned → Operational ↔ Inactive
/// → Retired → Disposed state machine: there is no fixed order any more,
/// and an Asset can be moved between ANY two of these statuses at any
/// time (including back from <see cref="OutOfFleet"/>).
/// </summary>
/// <remarks>
/// Persisted by name (see AssetConfiguration) — never rename a member
/// without a data migration.
/// </remarks>
public enum AssetStatus
{
    /// <summary>فعال — the Asset is in active use.</summary>
    Active = 1,

    /// <summary>آماده بکار — available and ready for work, not currently in use. A newly registered Asset starts here.</summary>
    Ready = 2,

    /// <summary>خارج از سرویس — temporarily unavailable (e.g. breakdown or repair).</summary>
    OutOfService = 3,

    /// <summary>خارج از ناوگان — removed from the fleet (retired, scrapped, sold, ...). It can be brought back to any other status.</summary>
    OutOfFleet = 4,
}
