using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Configuration.Presentation.Components;

/// <summary>
/// Persian display names for <see cref="PhysicalQuantityKind"/> (chat,
/// 2026-09-19), shared by the Unit of Measurement pages.
/// </summary>
internal static class PhysicalQuantityKindNames
{
    /// <summary>The kinds offered when registering or editing a unit, in display order.</summary>
    public static IReadOnlyList<PhysicalQuantityKind> Ordered { get; } =
    [
        PhysicalQuantityKind.Force,
        PhysicalQuantityKind.Dimension,
        PhysicalQuantityKind.Volume,
        PhysicalQuantityKind.Weight,
        PhysicalQuantityKind.Temperature,
        PhysicalQuantityKind.Electrical,
        PhysicalQuantityKind.FlowRate,
        PhysicalQuantityKind.Other,
    ];

    /// <summary>Gets the short display name (for tables).</summary>
    public static string GetName(PhysicalQuantityKind kind) => kind switch
    {
        PhysicalQuantityKind.Force => "نیرو",
        PhysicalQuantityKind.Dimension => "ابعاد",
        PhysicalQuantityKind.Volume => "حجم",
        PhysicalQuantityKind.Weight => "وزن",
        PhysicalQuantityKind.Temperature => "دما",
        PhysicalQuantityKind.Electrical => "الکتریکی",
        PhysicalQuantityKind.FlowRate => "دبی",
        PhysicalQuantityKind.Other => "سایر",
        _ => kind.ToString()
    };

    /// <summary>Gets the display name with examples (for selection lists).</summary>
    public static string GetSelectLabel(PhysicalQuantityKind kind) => kind switch
    {
        PhysicalQuantityKind.Force => "نیرو (توان، قدرت، فشار، گشتاور)",
        PhysicalQuantityKind.Dimension => "ابعاد (طول، عرض، ارتفاع، مساحت)",
        PhysicalQuantityKind.Volume => "حجم (لیتر، متر مکعب، سی‌سی و ...)",
        PhysicalQuantityKind.Weight => "وزن (جرم)",
        PhysicalQuantityKind.Temperature => "دما",
        PhysicalQuantityKind.Electrical => "الکتریکی (آمپر، ولت و ...)",
        PhysicalQuantityKind.FlowRate => "دبی (شدت جریان سیالات)",
        PhysicalQuantityKind.Other => "سایر",
        _ => kind.ToString()
    };
}
