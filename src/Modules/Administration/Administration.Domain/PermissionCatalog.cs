namespace Administration.Domain;

/// <summary>
/// The closed catalog of Sections and Actions available in the
/// Profile-creation permission matrix (chat, 2026-08-23).
/// </summary>
/// <remarks>
/// This is presentation-support data only (which checkboxes to show),
/// not an enforcement mechanism — actual authorization still happens
/// via <see cref="MachineryManager.SharedKernel.Abstractions.IPermissionEvaluator"/>
/// checking plain permission strings. Adding a section here does NOT
/// automatically enforce anything in the owning module; each module's
/// Command Handlers must independently check for the permission string
/// that corresponds to the action being performed.
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
        new PermissionSection("Asset", "دارایی‌ها (به‌زودی)"),
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
public sealed record PermissionSection(string Key, string DisplayName);