using System.Globalization;

namespace MachineryManagerEnterprise.Asset.Presentation.Components;

/// <summary>
/// Number helpers for the Asset edit pages (chat, 2026-09-19).
/// </summary>
internal static class NumberFormatting
{
    /// <summary>
    /// Removes insignificant trailing zeros from a decimal that came from a
    /// fixed-scale database column, so 12.5000 is shown as 12.5 and
    /// 3.0000 as 3 in an input field, while 12.3400 becomes 12.34.
    /// </summary>
    public static decimal? TrimTrailingZeros(decimal? value) =>
        value is { } v
            ? decimal.Parse(v.ToString("G29", CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture)
            : null;
}
