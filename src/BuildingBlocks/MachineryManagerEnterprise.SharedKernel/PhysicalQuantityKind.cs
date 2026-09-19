namespace MachineryManagerEnterprise.SharedKernel;

/// <summary>انواع کمیت‌های فیزیکی</summary>
public enum PhysicalQuantityKind
{
    /// <summary>سایر موارد / نامشخص</summary>
    Other = 0,

    /// <summary>نیرو، توان، قدرت، فشار، گشتاور </summary>
    Force = 1,
    
    /// <summary>ابعاد: طول، عرض، ارتفاع، مساحت (حجم جداست — <see cref="Volume"/>)</summary>
    Dimension = 2,
    
    /// <summary>وزن، جرم</summary>
    Weight = 3,
    
    /// <summary>دما</summary>
    Temperature = 4,
    
    /// <summary>الکتریکی: آمپر، ولت و ...</summary>
    Electrical = 5,
    
    /// <summary>دبی، شدت جریان سیالات</summary>
    FlowRate = 6,

    /// <summary>حجم: لیتر، متر مکعب، سی‌سی و ... (جدا شده از Dimension — chat, 2026-09-19)</summary>
    /// <remarks>Appended at the end so the numeric values of existing members do not change.</remarks>
    Volume = 7,
}
