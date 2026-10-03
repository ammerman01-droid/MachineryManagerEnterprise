namespace MachineryManagerEnterprise.Usage.Presentation.Components;

/// <summary>Maps a Meter Device's status (by its wire name) to a Persian display label — mirrors Asset.Presentation's <c>AssetStatusDisplay</c>.</summary>
public static class MeterDeviceStatusDisplay
{
    /// <summary>Returns the Persian display name for a Meter Device status name (Registered, Installed, Operational, Failed, Removed, or Archived).</summary>
    public static string GetName(string status) => status switch
    {
        "Registered" => "ثبت‌شده",
        "Installed" => "نصب‌شده",
        "Operational" => "در حال کار",
        "Failed" => "خراب",
        "Removed" => "جداشده",
        "Archived" => "بایگانی‌شده",
        _ => status,
    };

    /// <summary>Returns the MudBlazor color that best represents a Meter Device status, for a status chip.</summary>
    public static MudBlazor.Color GetColor(string status) => status switch
    {
        "Operational" => MudBlazor.Color.Success,
        "Installed" => MudBlazor.Color.Info,
        "Failed" => MudBlazor.Color.Error,
        "Removed" => MudBlazor.Color.Warning,
        "Archived" => MudBlazor.Color.Dark,
        _ => MudBlazor.Color.Default,
    };
}
