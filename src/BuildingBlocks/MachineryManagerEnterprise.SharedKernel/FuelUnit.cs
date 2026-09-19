namespace MachineryManagerEnterprise.SharedKernel;

/// <summary>
/// The fixed set of units in which an Asset's fuel consumption can be
/// counted (chat, 2026-09-19). A closed enum rather than a reference
/// into the Configuration module's UnitOfMeasurement table — these
/// values are needed to convert and total fuel quantities, so they
/// must not be user-editable. Lives in SharedKernel, alongside
/// <see cref="FuelKind"/>, so other modules can reference it.
/// </summary>
/// <remarks>
/// Extend this deliberately — adding a member here is a cross-module
/// contract change. Values are persisted by name, so never rename a
/// member without a data migration.
/// </remarks>
public enum FuelUnit
{
    /// <summary>Liter (لیتر).</summary>
    Liter = 0,

    /// <summary>US liquid gallon, ≈ 3.785 L (گالن).</summary>
    Gallon = 1,

    /// <summary>Kilogram (کیلوگرم) — typical for LPG/CNG.</summary>
    Kilogram = 2,

    /// <summary>Cubic meter (متر مکعب) — typical for natural gas/CNG.</summary>
    CubicMeter = 3,
}
