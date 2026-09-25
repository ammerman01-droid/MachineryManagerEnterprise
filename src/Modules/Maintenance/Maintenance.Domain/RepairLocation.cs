namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>
/// Where a Work Order's repair is predicted to be carried out (chat,
/// 2026-09-22).
/// </summary>
/// <remarks>Persisted by name — never rename a member without a data migration.</remarks>
public enum RepairLocation
{
    /// <summary>محل کار دستگاه — on-site, where the Asset currently stands.</summary>
    AssetSite = 1,

    /// <summary>تعمیرگاه مستقر در کارگاه — the workshop based at the work site.</summary>
    SiteWorkshop = 2,

    /// <summary>تعمیرگاه مرکزی.</summary>
    CentralWorkshop = 3,

    /// <summary>تعمیرگاه خارج از شرکت در منطقه — an external workshop in the region.</summary>
    RegionalExternalWorkshop = 4,

    /// <summary>تعمیرگاه خارج از شرکت — an external workshop outside the region.</summary>
    ExternalWorkshop = 5,
}
