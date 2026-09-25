namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>The urgency of a Work Order's repair (chat, 2026-09-22).</summary>
/// <remarks>Persisted by name — never rename a member without a data migration.</remarks>
public enum RepairPriority
{
    /// <summary>فوری.</summary>
    Urgent = 1,

    /// <summary>عادی.</summary>
    Normal = 2,
}
