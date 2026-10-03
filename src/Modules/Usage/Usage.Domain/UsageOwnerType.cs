namespace Usage.Domain;

/// <summary>
/// The kind of business object a Meter Device (or a
/// <see cref="UsageLedger"/>) is currently mounted on / measuring.
/// A Meter Device is not always mounted directly on an Asset: a
/// Tracked Component (BR-004 — e.g. a hydraulic breaker attachment,
/// or a secondary engine such as a concrete mixer's drum motor) can
/// carry its own Meter Device and its own accumulated usage,
/// independent of whichever Asset it is currently installed on
/// (chat, 2026-09-12). Stored as a plain discriminator rather than a
/// typed reference because Usage.Domain must not depend on
/// Asset.Domain or the future Component module (Modular Monolith
/// boundary — same pattern as Asset.ColorId referencing Configuration).
/// </summary>
public enum UsageOwnerType
{
    /// <summary>The owner is an Asset (Asset.Domain.Asset).</summary>
    Asset = 0,

    /// <summary>
    /// The owner is a Tracked Component (BR-004) — e.g. a hydraulic
    /// attachment or a secondary engine. The Component module is not
    /// yet implemented (chat, 2026-09-12); the reference is stored
    /// today and will be validated by the Application layer once that
    /// module exists.
    /// </summary>
    Component = 1,
}
