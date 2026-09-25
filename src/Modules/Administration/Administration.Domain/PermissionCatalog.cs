namespace Administration.Domain;

/// <summary>
/// The closed catalog of Sections and Actions available in the
/// Profile-creation permission matrix (chat, 2026-08-23).
/// </summary>
/// <remarks>
/// This is presentation-support data only (which checkboxes to show),
/// not an enforcement mechanism — actual authorization still happens
/// via <see cref="MachineryManagerEnterprise.SharedKernel.Abstractions.IPermissionEvaluator"/>
/// checking plain permission strings. Adding a section here does NOT
/// automatically enforce anything in the owning module; each module's
/// Command Handlers must independently check for the permission string
/// that corresponds to the action being performed.
/// AuditLog note (chat, 2026-09-06, gam 5): the AuditLog section is
/// read-only — only its "AuditLog.View" permission is ever enforced
/// (by the AuditLog module's query handler). The other three actions
/// appear in the matrix UI but are intentionally never checked.
/// FuelConsumption section added (chat, 2026-09-22) — its four actions
/// ("FuelConsumption.View/Create/Edit/Delete") are all genuinely
/// enforced by the Consumption module's Command/Query Handlers, unlike
/// AuditLog above.
/// </remarks>
public static class PermissionCatalog
{
    /// <summary>The four standard actions available for every section.</summary>
    public static IReadOnlyList<string> Actions { get; } = ["View", "Create", "Edit", "Delete"];

    /// <summary>The closed list of application sections shown in the permission matrix.</summary>
    public static IReadOnlyList<PermissionSection> Sections { get; } =
    [
        new PermissionSection("Holding", "هلدینگ"),
        new PermissionSection("Organization", "شرکت"),
        new PermissionSection("Project", "پروژه"),
        new PermissionSection("User", "کاربران"),
        new PermissionSection("Profile", "پروفایل‌ها"),
        new PermissionSection("Asset", "دارایی‌ها"),
        new PermissionSection("Color", "رنگ‌ها"),
        new PermissionSection("UnitOfMeasurement", "واحدهای اندازه‌گیری"),
        new PermissionSection("Company", "شرکت‌های سازنده"),
        new PermissionSection("FuelType", "انواع سوخت"),
        new PermissionSection("FuelConsumption", "سوخت‌گیری"),
        new PermissionSection("AuditLog", "لاگ فعالیت‌ها", ["View"]),
        new PermissionSection("Personnel", "پرسنل"),
        new PermissionSection("DrivingLicenseType", "گواهینامه‌ها"),
        new PermissionSection("JobTitle", "عناوین شغلی"),
        new PermissionSection("WorkCalendar", "تقویم کاری"),
        new PermissionSection("LubricantType", "انواع روانکار"),
        new PermissionSection("OverflowComponent", "قسمت‌های سرریز"),
        new PermissionSection("LubricantOverflowReport", "گزارش سرریز روانکار"),
        new PermissionSection("WorkOrder", "دستورهای کار", ["View", "Create", "Edit"]),
        new PermissionSection("ConsumptionFreezeSetting", "قفل گزارش‌های مصرف", ["View", "Edit"]),
    ];

    /// <summary>Builds the canonical permission string for a section/action pair (e.g. "Organization.Create").</summary>
    /// <param name="sectionKey">The section's key (e.g. "Organization").</param>
    /// <param name="action">The action name (e.g. "Create").</param>
    /// <returns>The permission string.</returns>
    public static string BuildPermission(string sectionKey, string action) => $"{sectionKey}.{action}";
}

/// <summary>A single row in the permission matrix.</summary>
/// <param name="Key">The section's key, used to build permission strings (e.g. "Organization").</param>
/// <param name="DisplayName">The section's Persian display label.</param>
/// <param name="Actions">
/// The actions this section offers in the matrix, for read-only modules
/// that support fewer than the four standard actions (e.g. AuditLog only
/// "View"). <c>null</c> means all four standard actions (gam 5 follow-up,
/// chat 2026-09-06).
/// </param>
public sealed record PermissionSection(
    string Key,
    string DisplayName,
    IReadOnlyList<string>? Actions = null)
{
    /// <summary>The actions the permission matrix must render for this section.</summary>
    public IReadOnlyList<string> EffectiveActions =>
        Actions ?? PermissionCatalog.Actions;
}
