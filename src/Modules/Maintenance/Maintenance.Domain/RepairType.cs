namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>Whether a Work Order's anticipated repair is major or minor (chat, 2026-09-22).</summary>
/// <remarks>Persisted by name — never rename a member without a data migration.</remarks>
public enum RepairType
{
    /// <summary>اساسی.</summary>
    Major = 1,

    /// <summary>جزئی.</summary>
    Minor = 2,
}
