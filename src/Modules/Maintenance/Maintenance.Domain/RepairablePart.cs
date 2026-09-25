namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>
/// The single main Asset part a Work Order suspects needs repair (chat,
/// 2026-09-22) — single-select; the actual Repair Execution (a later
/// phase) may cover more or less than this.
/// </summary>
/// <remarks>Persisted by name — never rename a member without a data migration.</remarks>
public enum RepairablePart
{
    /// <summary>موتور.</summary>
    Engine = 1,

    /// <summary>گیربکس.</summary>
    Gearbox = 2,

    /// <summary>هیدرولیک.</summary>
    Hydraulic = 3,

    /// <summary>زیربندی (شاسی).</summary>
    Chassis = 4,

    /// <summary>برق.</summary>
    Electrical = 5,

    /// <summary>بدنه.</summary>
    Body = 6,

    /// <summary>سایر.</summary>
    Other = 7,
}
