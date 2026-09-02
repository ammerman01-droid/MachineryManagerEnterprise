namespace MachineryManager.SharedKernel;

/// <summary>انواع کمیت‌های فیزیکی</summary>
public enum PhysicalQuantityKind
{
    /// <summary>سایر موارد / نامشخص</summary>
    Other = 0,

    /// <summary>نیرو، توان، قدرت، فشار، گشتاور </summary>
    Force = 1,
    
    /// <summary>ابعاد: طول، مساحت، حجم</summary>
    Dimension = 2,
    
    /// <summary>وزن، جرم</summary>
    Weight = 3,
    
    /// <summary>دما</summary>
    Temperature = 4,
    
    /// <summary>الکتریکی: آمپر، ولت و ...</summary>
    Electrical = 5,
    
    /// <summary>دبی، شدت جریان سیالات</summary>
    FlowRate = 6,
}