namespace MachineryManagerEnterprise.UI.Theming;

/// <summary>
/// The set of colour schemes the user can choose from. Combined with
/// <see cref="AppCornerStyle"/>, this fully determines the
/// <c>MudTheme</c> handed to <c>MudThemeProvider</c>.
/// </summary>
public enum AppThemeMode
{
    /// <summary>Light background with the standard MudBlazor accent palette.</summary>
    Light,

    /// <summary>Dark background, suited for low-light environments.</summary>
    Dark,

    /// <summary>The project's branded, higher-saturation light palette.</summary>
    Colorful
}