namespace MachineryManagerEnterprise.Consumption.Domain;

/// <summary>
/// Identifies which of an Asset's (up to two) configured fuel slots a
/// <see cref="FuelConsumption"/> record refers to (chat, 2026-09-16).
/// A single record always covers exactly one slot — refueling both
/// tanks of a bi-fuel Asset in one visit requires two separate
/// records.
/// </summary>
public enum FuelSlot
{
    /// <summary>The Asset's primary fuel (Asset.PrimaryFuelKind / PrimaryFuelUnit).</summary>
    Primary = 1,

    /// <summary>The Asset's secondary fuel, for bi-fuel Assets (Asset.SecondaryFuelKind / SecondaryFuelUnit).</summary>
    Secondary = 2
}
