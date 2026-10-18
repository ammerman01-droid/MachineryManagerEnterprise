using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain;

/// <summary>
/// The system-wide daily Operational Usage limits (chat, 2026-10-13): the most usage one calendar
/// day may total on a Usage Ledger before a registration is rejected as implausible. A Meter
/// Device may carry its own <see cref="MeterDevice.DailyCapOverride"/> that replaces the default.
/// </summary>
public static class UsageCapDefaults
{
    /// <summary>The default daily limit for hour meters — a day has 24 hours.</summary>
    public const decimal HourlyDailyCap = 24m;

    /// <summary>The default daily limit for kilometer odometers.</summary>
    public const decimal KilometerDailyCap = 1000m;

    /// <summary>The default daily limit for mile odometers — about the same distance as <see cref="KilometerDailyCap"/> kilometers.</summary>
    public const decimal MileDailyCap = 620m;

    /// <summary>
    /// Resolves the daily limit to enforce: the device's own override when it has one, otherwise
    /// the default for <paramref name="unit"/>.
    /// </summary>
    /// <param name="unit">The unit the Ledger tracks.</param>
    /// <param name="deviceOverride">The source device's <see cref="MeterDevice.DailyCapOverride"/>, if any.</param>
    /// <returns>The maximum Operational Usage one calendar day may total.</returns>
    public static decimal GetDailyCap(MeterReadingUnit unit, decimal? deviceOverride) =>
        deviceOverride ?? unit switch
        {
            MeterReadingUnit.Hour => HourlyDailyCap,
            MeterReadingUnit.Kilometer => KilometerDailyCap,
            MeterReadingUnit.Mile => MileDailyCap,
            _ => decimal.MaxValue,
        };
}
