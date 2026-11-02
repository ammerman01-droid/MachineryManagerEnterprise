namespace Administration.Domain;

/// <summary>
/// Identifies the permission sections whose data is Holding-level reference data (colors,
/// fuel types, units of measurement, ...). Every Organization and Project of a Holding works
/// with this data (it fills their dropdowns) although it belongs to the Holding itself, so the
/// strictly top-down scope rule is relaxed for it: an Organization-level assignment covers it
/// like any other of its resources, a Project-level assignment may only read it.
/// </summary>
public static class HoldingReferenceData
{
    private static readonly HashSet<string> Sections = new(StringComparer.Ordinal)
    {
        "Color",
        "UnitOfMeasurement",
        "Company",
        "FuelType",
        "DrivingLicenseType",
        "JobTitle",
        "LubricantType",
        "OverflowComponent",
    };

    /// <summary>The only action a Project-level assignment may exercise on reference data.</summary>
    public const string ReadOnlyAction = "View";

    /// <summary>
    /// Tries to read a permission string ("Section.Action") that belongs to a reference-data section.
    /// </summary>
    /// <param name="permission">The permission string, e.g. "Color.View".</param>
    /// <param name="action">The action part, when the section is reference data.</param>
    /// <returns><see langword="true"/> when the permission belongs to a Holding reference-data section.</returns>
    public static bool TryGetAction(string permission, out string action)
    {
        action = string.Empty;
        var separator = permission.LastIndexOf('.');

        if (separator <= 0 || separator == permission.Length - 1)
        {
            return false;
        }

        if (!Sections.Contains(permission[..separator]))
        {
            return false;
        }

        action = permission[(separator + 1)..];
        return true;
    }
}
