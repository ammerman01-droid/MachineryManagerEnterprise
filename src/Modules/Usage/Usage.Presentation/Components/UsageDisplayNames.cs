namespace MachineryManagerEnterprise.Usage.Presentation.Components;

/// <summary>Maps Usage-module enum wire names to Persian display labels — mirrors Asset.Presentation's <c>UnitDisplayNames</c>.</summary>
public static class UsageDisplayNames
{
    /// <summary>Returns the Persian display name for a reading unit name (Hour, Kilometer, or Mile), or "—" when not yet established (BR-005).</summary>
    public static string GetUnitName(string? unit) => unit switch
    {
        "Hour" => "ساعت",
        "Kilometer" => "کیلومتر",
        "Mile" => "مایل",
        _ => "—",
    };

    /// <summary>Returns the Persian display name for an owner type name (Asset or Component).</summary>
    public static string GetOwnerTypeName(string? ownerType) => ownerType switch
    {
        "Asset" => "دارایی",
        "Component" => "قطعهٔ ردیابی‌شونده",
        _ => "—",
    };

    /// <summary>Returns the Persian display name for a usage entry kind (chat, 2026-09-29).</summary>
    public static string GetEntryKindName(string? kind) => kind switch
    {
        "ShiftReading" => "ثبت شیفت",
        "Rebase" => "بازتنظیم شمارنده",
        _ => "—",
    };

    /// <summary>Returns the Persian display name for a usage entry origin (chat, 2026-09-29).</summary>
    public static string GetEntryOriginName(string? origin) => origin switch
    {
        "Manual" => "دستی",
        "AutoFilled" => "پرشدهٔ خودکار",
        _ => "—",
    };
}
