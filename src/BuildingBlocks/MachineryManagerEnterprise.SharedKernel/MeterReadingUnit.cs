namespace MachineryManagerEnterprise.SharedKernel;

/// <summary>
/// The fixed set of units in which an Asset's counter/odometer is read
/// (chat, 2026-09-19). A closed enum rather than a reference into the
/// Configuration module's UnitOfMeasurement table. Lives in
/// SharedKernel, alongside <see cref="FuelKind"/>, so other modules
/// (e.g. usage or fuel logs) can reference it.
/// </summary>
/// <remarks>
/// Extend this deliberately — adding a member here is a cross-module
/// contract change. Values are persisted by name, so never rename a
/// member without a data migration.
/// </remarks>
public enum MeterReadingUnit
{
    /// <summary>Operating hours (ساعت).</summary>
    Hour = 0,

    /// <summary>Kilometers (کیلومتر).</summary>
    Kilometer = 1,

    /// <summary>Miles (مایل).</summary>
    Mile = 2,
}
