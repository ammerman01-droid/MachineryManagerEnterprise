using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.SharedKernel.Infrastructure;

/// <summary>
/// Shared parse/format helpers for the theme preference cookie value, used
/// by both <see cref="CookieThemePreferenceStore"/> and
/// <see cref="CompositeThemePreferenceStore"/> so the encoding lives in
/// exactly one place.
/// </summary>
internal static class ThemeCookieCodec
{
    /// <summary>The cookie name both stores read from and write to.</summary>
    public const string CookieName = "mme_theme";

    /// <summary>
    /// Parses a raw cookie value into a <see cref="ThemePreference"/>, or
    /// <see langword="null"/> if it is malformed.
    /// </summary>
    /// <param name="raw">The raw cookie value.</param>
    public static ThemePreference? Parse(string raw)
    {
        var parts = raw.Split('|');
        return parts.Length == 2 ? new ThemePreference(parts[0], parts[1]) : null;
    }

    /// <summary>Formats a <see cref="ThemePreference"/> into the raw cookie value.</summary>
    /// <param name="preference">The preference to format.</param>
    public static string Format(ThemePreference preference) => $"{preference.Mode}|{preference.CornerStyle}";
}