namespace MachineryManagerEnterprise.Asset.Presentation.Components;

/// <summary>
/// Persian display names for the Asset statuses (chat, 2026-09-20),
/// shared by the Asset pages. The API returns the status by its enum name.
/// </summary>
internal static class AssetStatusDisplay
{
    /// <summary>The selectable statuses (enum member names), in display order.</summary>
    public static IReadOnlyList<string> Values { get; } = ["Active", "Ready", "OutOfService", "OutOfFleet"];

    /// <summary>Gets the Persian display name of a status given its enum member name, or "-" if not set.</summary>
    public static string GetName(string? status) => status switch
    {
        "Active" => "فعال",
        "Ready" => "آماده بکار",
        "OutOfService" => "خارج از سرویس",
        "OutOfFleet" => "خارج از ناوگان",
        null or "" => "-",
        _ => status
    };
}
