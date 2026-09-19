using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Asset.Presentation.Components;

/// <summary>
/// Persian display names for the fixed <see cref="FuelUnit"/> and
/// <see cref="MeterReadingUnit"/> enums (chat, 2026-09-19), shared by
/// the Asset pages so the labels are defined in one place.
/// </summary>
internal static class UnitDisplayNames
{
    /// <summary>Gets the display name of a fuel counting unit.</summary>
    public static string GetFuelUnitName(FuelUnit unit) => unit switch
    {
        FuelUnit.Liter => "لیتر",
        FuelUnit.Gallon => "گالن",
        FuelUnit.Kilogram => "کیلوگرم",
        FuelUnit.CubicMeter => "متر مکعب",
        _ => unit.ToString()
    };

    /// <summary>Gets the display name of a fuel counting unit given its enum member name (as returned by the API), or "-" if not set.</summary>
    public static string GetFuelUnitName(string? unitName) =>
        string.IsNullOrEmpty(unitName)
            ? "-"
            : Enum.TryParse<FuelUnit>(unitName, out var unit) ? GetFuelUnitName(unit) : unitName;

    /// <summary>Gets the display name of a meter (counter/odometer) reading unit.</summary>
    public static string GetMeterReadingUnitName(MeterReadingUnit unit) => unit switch
    {
        MeterReadingUnit.Hour => "ساعت",
        MeterReadingUnit.Kilometer => "کیلومتر",
        MeterReadingUnit.Mile => "مایل",
        _ => unit.ToString()
    };

    /// <summary>Gets the display name of a meter reading unit given its enum member name (as returned by the API), or "-" if not set.</summary>
    public static string GetMeterReadingUnitName(string? unitName) =>
        string.IsNullOrEmpty(unitName)
            ? "-"
            : Enum.TryParse<MeterReadingUnit>(unitName, out var unit) ? GetMeterReadingUnitName(unit) : unitName;
}
