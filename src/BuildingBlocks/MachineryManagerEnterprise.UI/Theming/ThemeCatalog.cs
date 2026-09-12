using MudBlazor;

namespace MachineryManagerEnterprise.UI.Theming;

/// <summary>
/// Builds the concrete <see cref="MudTheme"/> instance for a given
/// combination of <see cref="AppThemeMode"/> and <see cref="AppCornerStyle"/>.
/// This is the single place where the application's palettes and corner
/// radii are defined; nothing outside this class should hardcode colours.
/// </summary>
public static class ThemeCatalog
{
    /// <summary>Border radius applied when <see cref="AppCornerStyle.Sharp"/> is selected.</summary>
    private const string SharpRadius = "0px";

    /// <summary>Border radius applied when <see cref="AppCornerStyle.Rounded"/> is selected.</summary>
    private const string RoundedRadius = "12px";

    /// <summary>
    /// Builds the <see cref="MudTheme"/> for the requested mode and corner
    /// style. A new instance is returned on every call; <see cref="ThemeService"/>
    /// is responsible for caching the result as <c>CurrentTheme</c>.
    /// </summary>
    /// <param name="mode">Which colour scheme to build.</param>
    /// <param name="cornerStyle">Which corner treatment to apply.</param>
    public static MudTheme Build(AppThemeMode mode, AppCornerStyle cornerStyle) =>
        new()
        {
            PaletteLight = GetLightPalette(mode),
            PaletteDark = GetDarkPalette(mode),
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = cornerStyle == AppCornerStyle.Sharp ? SharpRadius : RoundedRadius
            }
        };

    /// <summary>
    /// Returns the palette used when <c>MudThemeProvider.IsDarkMode</c> is
    /// <see langword="false"/> — i.e. for <see cref="AppThemeMode.Light"/>
    /// and <see cref="AppThemeMode.Colorful"/>.
    /// </summary>
    /// <param name="mode">The selected theme mode.</param>
    private static PaletteLight GetLightPalette(AppThemeMode mode) => mode switch
    {
        AppThemeMode.Colorful => new PaletteLight
        {
            Primary = "#7C4DFF",
            Secondary = "#FF6E40",
            AppbarBackground = "#7C4DFF",
            Background = "#FBFAFF"
        },
        _ => new PaletteLight()
    };

    /// <summary>
    /// Returns the palette used when <c>MudThemeProvider.IsDarkMode</c> is
    /// <see langword="true"/> — i.e. for <see cref="AppThemeMode.Dark"/>.
    /// </summary>
    /// <param name="mode">The selected theme mode.</param>
    private static PaletteDark GetDarkPalette(AppThemeMode mode) => mode switch
    {
        AppThemeMode.Colorful => new PaletteDark
        {
            Primary = "#B388FF",
            Secondary = "#FFAB91"
        },
        _ => new PaletteDark()
    };
}