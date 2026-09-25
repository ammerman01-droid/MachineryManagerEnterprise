namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>
/// The lifecycle status of a Work Order (chat, 2026-09-22): created
/// (<see cref="Open"/>), then either cancelled with a reason or
/// converted to repair. Once <see cref="Cancelled"/> or
/// <see cref="InRepair"/>, a Work Order is no longer editable or
/// cancellable — a Work Order results in exactly one repair; leftover
/// or extra work needs a new Work Order.
/// </summary>
/// <remarks>
/// Persisted by name (see WorkOrderConfiguration) — never rename a
/// member without a data migration.
/// </remarks>
public enum WorkOrderStatus
{
    /// <summary>باز — newly created, editable, cancellable, and convertible to repair.</summary>
    Open = 1,

    /// <summary>لغو شده — cancelled, with a recorded reason. Terminal.</summary>
    Cancelled = 2,

    /// <summary>در حال تعمیر — converted to repair. Terminal for this Work Order; the actual repair work is recorded in the Repair Execution part (a later phase).</summary>
    InRepair = 3,
}
