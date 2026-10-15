using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MachineryManagerEnterprise.Usage.Presentation.Components;

/// <summary>
/// Turns the API's error responses into messages a user can act on (chat, 2026-10-12). The server
/// reports problems as a machine-readable <c>errorCode</c> plus an English <c>message</c>; this
/// helper maps each Usage error code to a Persian explanation, pulling the dates/values out of the
/// English message where they make the explanation more useful. Codes it does not know fall back to
/// the server's own text, so nothing is ever hidden.
/// </summary>
public static class UsageProblemMessages
{
    private static readonly PersianCalendar PersianCalendar = new();

    /// <summary>
    /// Reads a failed <paramref name="response"/> and returns the text to show the user.
    /// </summary>
    /// <param name="response">The unsuccessful HTTP response.</param>
    /// <param name="fallbackPrefix">Prefix used only when the problem is not recognized (e.g. "خطا در ثبت کارکرد").</param>
    /// <returns>A Persian, user-facing description of what went wrong.</returns>
    public static async Task<string> DescribeAsync(HttpResponseMessage response, string fallbackPrefix)
    {
        var body = await response.Content.ReadAsStringAsync();

        string? code = null;
        string? message = null;
        var details = new List<string>();

        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;

            if (root.ValueKind == JsonValueKind.Object)
            {
                code = ReadString(root, "errorCode") ?? ReadString(root, "code");
                message = ReadString(root, "message") ?? ReadString(root, "detail") ?? ReadString(root, "title");

                CollectDetails(root, "details", details);
                CollectDetails(root, "errors", details);
            }
        }
        catch (JsonException)
        {
            // Not a JSON problem document — fall through to the status-based messages below.
        }

        if (code is not null && message is not null && Translate(code, message) is { } translated)
        {
            return translated;
        }

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            return "شما مجوز انجام این عملیات را ندارید.";
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return "نشست شما منقضی شده است؛ لطفاً دوباره وارد شوید.";
        }

        if (details.Count > 0)
        {
            return $"اطلاعات واردشده معتبر نیست: {string.Join("؛ ", details)}";
        }

        return string.IsNullOrWhiteSpace(message)
            ? $"{fallbackPrefix} (کد وضعیت {(int)response.StatusCode})"
            : $"{fallbackPrefix}: {message}";
    }

    private static string? Translate(string code, string message) => code switch
    {
        "UsageLedger.RawValueDecreased" => RawValueDecreased(message),
        "UsageLedger.NonSequentialShift" =>
            "برای این شیفت قبلاً کارکرد ثبت شده است، یا این شیفت قبل از آخرین شیفت ثبت‌شده قرار دارد. شیفت‌ها باید به‌ترتیب زمانی ثبت شوند.",
        "UsageLedger.GapRequiresManualEntry" => GapRequiresManualEntry(message),
        "UsageLedger.DifferentSourceDeviceRequiresRebase" =>
            "آخرین قرائت ثبت‌شده برای این دارایی با دستگاه دیگری انجام شده است. اگر کنتور تعویض شده، ابتدا از «بازتنظیم شمارنده» استفاده کنید.",
        "UsageLedger.EntryFrozen" => EntryFrozen(message),
        "UsageLedger.RebaseDateNotAfterLastEntry" =>
            "تاریخ بازتنظیم باید بعد از تاریخ آخرین ردیف ثبت‌شده باشد.",
        "UsageLedger.RebaseDateInFuture" =>
            "تاریخ بازتنظیم نمی‌تواند در آینده باشد.",
        "UsageLedger.OnlyLatestEntryDeletable" =>
            "فقط آخرین ردیف ثبت‌شده قابل حذف است؛ ابتدا ردیف‌های جدیدتر را حذف کنید.",
        "UsageLedger.NoEntriesToDelete" => "ردیفی برای حذف وجود ندارد.",
        "UsageLedger.EntryNotFound" => "ردیفی برای این تاریخ و شیفت پیدا نشد.",
        "UsageLedger.NotFound" => "برای این دارایی هنوز کارکردی ثبت نشده است.",
        "UsageLedger.FreezeBoundaryCannotMoveBackward" =>
            "تاریخ قفل فقط می‌تواند به جلو برود و به تاریخ قبل‌تر برنمی‌گردد.",
        "UsageLedger.NotAuthorized" or "MeterDevice.NotAuthorized" =>
            "شما مجوز انجام این عملیات را ندارید.",
        "MeterDevice.NegativeReadingValue" => "مقدار کنتور نمی‌تواند منفی باشد.",
        "MeterDevice.NotReadyForReading" =>
            "وضعیت فعلی دستگاه اجازهٔ ثبت قرائت نمی‌دهد؛ دستگاه باید نصب‌شده یا فعال باشد.",
        "MeterDevice.NotFound" => "دستگاه پیدا نشد.",
        "MeterDevice.OwnerNotSupported" =>
            "این دستگاه روی یک دارایی نصب نشده است؛ ابتدا آن را روی دارایی نصب کنید.",
        "MeterDevice.NotInstalledOnOwner" =>
            "دستگاه جایگزین باید ابتدا روی همین دارایی نصب شود.",
        "MeterDevice.UnitMismatch" => "واحد اندازه‌گیری دستگاه با واحد دارایی هم‌خوانی ندارد.",
        "MeterDevice.InvalidTransition" => "این عملیات در وضعیت فعلی دستگاه مجاز نیست.",
        "Asset.NotFound" => "دارایی پیدا نشد.",
        "Usage.CalendarNotResolved" => CalendarNotResolved(message),
        "Usage.ShiftNotScheduled" => ShiftNotScheduled(message),
        _ => null,
    };

    private static string RawValueDecreased(string message)
    {
        const string advice = "قرائت کنتور نمی‌تواند از آخرین قرائت ثبت‌شده کمتر باشد. اگر کنتور تعویض یا صفر شده است، از «بازتنظیم شمارنده» استفاده کنید.";
        var match = Regex.Match(message, @"Raw value (?<new>\S+) is lower than the previously recorded value (?<old>\S+) for");

        return match.Success
            ? $"مقدار واردشده ({match.Groups["new"].Value}) از آخرین قرائت ثبت‌شده ({match.Groups["old"].Value}) کمتر است. {advice}"
            : advice;
    }

    private static string GapRequiresManualEntry(string message)
    {
        const string advice = "ابتدا کارکرد شیفت‌های جاافتاده را به‌ترتیب ثبت کنید؛ اگر کنتور تعویض شده، از «بازتنظیم شمارنده» استفاده کنید.";
        var match = Regex.Match(
            message,
            @"Shifts from (?<d1>\d{4}-\d{2}-\d{2}) \(shift (?<s1>\d+)\) to (?<d2>\d{4}-\d{2}-\d{2}) \(shift (?<s2>\d+)\)");

        if (!match.Success)
        {
            return $"بین آخرین ثبت و این ثبت، شیفت‌هایی گزارش نشده‌اند و مقدار کنتور تغییر کرده است. {advice}";
        }

        var firstShift = int.Parse(match.Groups["s1"].Value, CultureInfo.InvariantCulture) + 1;
        var lastShift = int.Parse(match.Groups["s2"].Value, CultureInfo.InvariantCulture) + 1;

        return $"شیفت‌های گزارش‌نشده از {ToJalali(match.Groups["d1"].Value)} (شیفت {firstShift}) تا {ToJalali(match.Groups["d2"].Value)} (شیفت {lastShift}) وجود دارند و مقدار کنتور تغییر کرده است. {advice}";
    }

    private static string EntryFrozen(string message)
    {
        var match = Regex.Match(message, @"frozen up to and including (?<f>\d{4}-\d{2}-\d{2})");

        return match.Success
            ? $"کارکردهای تا تاریخ {ToJalali(match.Groups["f"].Value)} قفل شده‌اند و ثبت، اصلاح یا حذف در آن‌ها ممکن نیست."
            : "این تاریخ قفل شده است و ثبت، اصلاح یا حذف در آن ممکن نیست.";
    }

    private static string CalendarNotResolved(string message)
    {
        var match = Regex.Match(message, @"(?<d>\d{4}-\d{2}-\d{2})");

        return match.Success
            ? $"برای {ToJalali(match.Groups["d"].Value)} در تقویم کاری پروژهٔ این دارایی روزی تعریف نشده است. ابتدا تقویم کاری پروژه را تکمیل کنید."
            : "برای این تاریخ در تقویم کاری پروژهٔ دارایی روزی تعریف نشده است. ابتدا تقویم کاری پروژه را تکمیل کنید.";
    }

    private static string ShiftNotScheduled(string message)
    {
        var match = Regex.Match(message, @"Shift (?<s>\d+) is not scheduled for (?<d>\d{4}-\d{2}-\d{2})");

        return match.Success
            ? $"شیفت {int.Parse(match.Groups["s"].Value, CultureInfo.InvariantCulture) + 1} در تقویم کاری {ToJalali(match.Groups["d"].Value)} وجود ندارد."
            : "شیفت انتخاب‌شده در تقویم کاری این تاریخ وجود ندارد.";
    }

    private static string ToJalali(string isoDate)
    {
        if (!DateTime.TryParseExact(isoDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return isoDate;
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"{PersianCalendar.GetYear(date):0000}/{PersianCalendar.GetMonth(date):00}/{PersianCalendar.GetDayOfMonth(date):00}");
    }

    private static string? ReadString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    /// <summary>
    /// Collects human-readable strings from a <c>details</c> array (strings or objects with a message)
    /// or from a validation <c>errors</c> object (property name to array of messages).
    /// </summary>
    private static void CollectDetails(JsonElement root, string propertyName, List<string> into)
    {
        if (!root.TryGetProperty(propertyName, out var value))
        {
            return;
        }

        switch (value.ValueKind)
        {
            case JsonValueKind.Array:
                foreach (var item in value.EnumerateArray())
                {
                    var text = item.ValueKind == JsonValueKind.String
                        ? item.GetString()
                        : item.ValueKind == JsonValueKind.Object
                            ? ReadString(item, "message") ?? ReadString(item, "errorMessage") ?? ReadString(item, "error")
                            : null;

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        into.Add(text);
                    }
                }

                break;

            case JsonValueKind.Object:
                foreach (var property in value.EnumerateObject())
                {
                    if (property.Value.ValueKind != JsonValueKind.Array)
                    {
                        continue;
                    }

                    foreach (var item in property.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(item.GetString()))
                        {
                            into.Add(item.GetString()!);
                        }
                    }
                }

                break;
        }
    }
}
