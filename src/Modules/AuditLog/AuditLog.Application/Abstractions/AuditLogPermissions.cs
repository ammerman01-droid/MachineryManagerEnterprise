namespace MachineryManager.AuditLog.Application.Abstractions;

/// <summary>
/// The permission strings enforced by the AuditLog module
/// (chat, 2026-09-06, gam 5).
/// </summary>
/// <remarks>
/// Deliberately a plain string constant, per the PermissionCatalog
/// convention: <c>PermissionCatalog.BuildPermission("AuditLog", "View")</c>
/// produces exactly this value in the Administration module, which
/// AuditLog.Application may not reference (Modular Monolith Rules).
/// </remarks>
public static class AuditLogPermissions
{
    /// <summary>The permission required to view the audit trail (section "AuditLog", action "View").</summary>
    public const string View = "AuditLog.View";
}