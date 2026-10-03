namespace Usage.Domain;

/// <summary>
/// The lifecycle state of a Meter Device (Section 4.9, State Machines):
/// Registered → Installed → Operational → Failed → Removed → Archived.
/// Removing never removes history (BR-013); replacing never resets
/// accumulated Operational Usage (BR-011).
/// </summary>
public enum MeterDeviceStatus
{
    /// <summary>Identity captured; not yet mounted on any owner.</summary>
    Registered = 0,

    /// <summary>Mounted on an owner (Asset or Component); no reading confirmed yet.</summary>
    Installed = 1,

    /// <summary>Mounted and confirmed producing valid readings.</summary>
    Operational = 2,

    /// <summary>Malfunctioning; readings are not accepted until recovered (BR-014).</summary>
    Failed = 3,

    /// <summary>Physically removed from its owner; history preserved (BR-013).</summary>
    Removed = 4,

    /// <summary>Final state — retired from the fleet; history remains accessible.</summary>
    Archived = 5,
}
