using System.Globalization;
using System.Text;

namespace MachineryManagerEnterprise.UI.Calendar;

/// <summary>
/// Provides stateless conversion, formatting, and calculation utilities
/// for the Jalali (Persian / Shamsi) calendar, backed entirely by the
/// built-in <see cref="System.Globalization.PersianCalendar"/>.
/// </summary>
/// <remarks>
/// Per ADR-0038, this class is a Presentation-layer-only concern.
/// It never leaks Jalali values outside <see cref="MachineryManagerEnterprise.UI"/>:
/// every public method that represents the value which is ultimately stored
/// works with standard Gregorian <see cref="DateTime"/>. No additional NuGet
/// package is used; all conversion math is provided by the .NET base class
/// library.
/// </remarks>
public static class JalaliCalendarConverter
{
    private static readonly PersianCalendar Calendar = new();

    /// <summary>
    /// Gets the official Persian month names, in calendar order
    /// (index 0 = فروردین, index 11 = اسفند).
    /// </summary>
    public static readonly string[] MonthNames =
    [
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
        "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
    ];

    /// <summary>
    /// Gets the short weekday names in Iranian week order
    /// (index 0 = شنبه ... index 6 = جمعه).
    /// </summary>
    public static readonly string[] WeekDayNamesShort =
    [
        "ش", "ی", "د", "س", "چ", "پ", "ج"
    ];

    /// <summary>
    /// Converts a Gregorian <see cref="DateTime"/> into its Jalali
    /// year, month, and day components.
    /// </summary>
    public static (int Year, int Month, int Day) ToJalali(DateTime gregorian) =>
        (Calendar.GetYear(gregorian), Calendar.GetMonth(gregorian), Calendar.GetDayOfMonth(gregorian));

    /// <summary>
    /// Converts a Jalali year, month, and day into the equivalent
    /// Gregorian <see cref="DateTime"/> (time component set to midnight).
    /// </summary>
    public static DateTime ToGregorian(int jalaliYear, int jalaliMonth, int jalaliDay) =>
        Calendar.ToDateTime(jalaliYear, jalaliMonth, jalaliDay, 0, 0, 0, 0);

    /// <summary>
    /// Determines whether the given Jalali year is a leap year.
    /// </summary>
    public static bool IsLeapYear(int jalaliYear) => Calendar.IsLeapYear(jalaliYear);

    /// <summary>
    /// Gets the number of days in the given Jalali year/month.
    /// </summary>
    public static int DaysInMonth(int jalaliYear, int jalaliMonth) => Calendar.GetDaysInMonth(jalaliYear, jalaliMonth);

    /// <summary>
    /// Gets the Gregorian day-of-week for a given Jalali calendar day.
    /// </summary>
    public static DayOfWeek GetGregorianDayOfWeek(int jalaliYear, int jalaliMonth, int jalaliDay) =>
        ToGregorian(jalaliYear, jalaliMonth, jalaliDay).DayOfWeek;

    /// <summary>
    /// Maps a .NET <see cref="DayOfWeek"/> (Sunday = 0) onto the Iranian
    /// week's column index (Saturday = 0 ... Friday = 6).
    /// </summary>
    public static int ToIranianWeekdayIndex(DayOfWeek dayOfWeek) => ((int)dayOfWeek + 1) % 7;

    /// <summary>
    /// Converts Latin/ASCII digits (0-9) in <paramref name="value"/> into
    /// their Persian equivalents (۰-۹). Non-digit characters are unchanged.
    /// </summary>
    public static string ToPersianDigits(string value)
    {
        var builder = new StringBuilder(value.Length);

        foreach (var ch in value)
        {
            builder.Append(ch is >= '0' and <= '9'
                ? (char)('۰' + (ch - '0'))
                : ch);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Converts Persian (۰-۹) or Arabic-Indic (٠-٩) digits in
    /// <paramref name="value"/> into their Latin/ASCII equivalents (0-9).
    /// Non-digit characters are unchanged.
    /// </summary>
    public static string ToLatinDigits(string value)
    {
        var builder = new StringBuilder(value.Length);

        foreach (var ch in value)
        {
            if (ch is >= '۰' and <= '۹')
            {
                builder.Append((char)('0' + (ch - '۰')));
            }
            else if (ch is >= '٠' and <= '٩')
            {
                builder.Append((char)('0' + (ch - '٠')));
            }
            else
            {
                builder.Append(ch);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Formats a Gregorian date as a Jalali date string using Persian
    /// digits, e.g. "۱۴۰۴/۰۷/۲۳". Returns an empty string for <c>null</c>.
    /// </summary>
    public static string Format(DateTime? gregorian)
    {
        if (gregorian is null)
        {
            return string.Empty;
        }

        var (year, month, day) = ToJalali(gregorian.Value);

        return ToPersianDigits($"{year:0000}/{month:00}/{day:00}");
    }

    /// <summary>
    /// Formats a UTC/offset-aware timestamp as a Jalali date and time string
    /// using Persian digits, e.g. "۱۴۰۴/۰۷/۲۳ ۱۴:۰۵:۰۹". The time portion is
    /// converted to local time before formatting; only the date portion uses
    /// the Jalali calendar (a clock's hour/minute/second is not
    /// calendar-dependent).
    /// </summary>
    public static string FormatDateTime(DateTimeOffset value)
    {
        var local = value.ToLocalTime();
        var (year, month, day) = ToJalali(local.DateTime);

        return ToPersianDigits($"{year:0000}/{month:00}/{day:00} {local:HH:mm:ss}");
    }

    /// <summary>
    /// Parses a Jalali date string (accepting either Persian or Latin
    /// digits, and '/' or '-' as separators) into the equivalent
    /// Gregorian <see cref="DateTime"/>.
    /// </summary>
    /// <returns>
    /// <c>null</c> when <paramref name="text"/> is empty or whitespace.
    /// </returns>
    /// <exception cref="FormatException">
    /// Thrown when <paramref name="text"/> is not a valid Jalali date.
    /// This is caught automatically by MudBlazor's <see cref="Converter{T,U}"/>
    /// and surfaced as a field validation error.
    /// </exception>
    public static DateTime? Parse(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var normalized = ToLatinDigits(text).Trim();
        var parts = normalized.Split(['/', '-'], StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3 ||
            !int.TryParse(parts[0], out var year) ||
            !int.TryParse(parts[1], out var month) ||
            !int.TryParse(parts[2], out var day))
        {
            throw new FormatException($"'{text}' یک تاریخ شمسی معتبر نیست.");
        }

        try
        {
            if (day < 1 || day > DaysInMonth(year, month))
            {
                throw new FormatException($"'{text}' یک تاریخ شمسی معتبر نیست.");
            }

            return ToGregorian(year, month, day);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new FormatException($"'{text}' یک تاریخ شمسی معتبر نیست.", ex);
        }
    }
}