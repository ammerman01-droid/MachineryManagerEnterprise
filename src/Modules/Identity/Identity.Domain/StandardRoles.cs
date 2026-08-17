namespace MachineryManager.Identity.Domain;

/// <summary>
/// The closed catalog of Standard Roles defined in 05-application,
/// Section 5.8 (Authorization Model).
/// </summary>
/// <remarks>
/// This list is closed to what is explicitly documented. Adding a new
/// role first requires updating the approved documentation (Section
/// 5.8), per the AI Engineering Contract's Business Rules governance
/// (Section 1.3: "Never invent business rules") — it is not introduced
/// here unilaterally.
/// </remarks>
public static class StandardRoles
{
    /// <summary>Platform-level administrator, scoped across all Organizations.</summary>
    public const string SystemAdministrator = "System Administrator";

    /// <summary>Administrator scoped to a single Organization (tenant).</summary>
    public const string OrganizationAdministrator = "Organization Administrator";

    /// <summary>Manages the asset fleet.</summary>
    public const string FleetManager = "Fleet Manager";

    /// <summary>Manages maintenance planning and operations.</summary>
    public const string MaintenanceManager = "Maintenance Manager";

    /// <summary>Performs maintenance activities.</summary>
    public const string MaintenanceTechnician = "Maintenance Technician";

    /// <summary>Supervises workshop operations.</summary>
    public const string WorkshopSupervisor = "Workshop Supervisor";

    /// <summary>Operates machinery assets.</summary>
    public const string Operator = "Operator";

    /// <summary>Manages financial records and expenses.</summary>
    public const string FinancialOfficer = "Financial Officer";

    /// <summary>Manages procurement activities.</summary>
    public const string ProcurementOfficer = "Procurement Officer";

    /// <summary>Manages document lifecycle and compliance.</summary>
    public const string DocumentController = "Document Controller";

    /// <summary>Read-only access for audit purposes.</summary>
    public const string ReadOnlyAuditor = "Read-Only Auditor";

    /// <summary>All standard role names, used for seeding and validation.</summary>
    public static IReadOnlyList<string> All { get; } =
    [
        SystemAdministrator,
        OrganizationAdministrator,
        FleetManager,
        MaintenanceManager,
        MaintenanceTechnician,
        WorkshopSupervisor,
        Operator,
        FinancialOfficer,
        ProcurementOfficer,
        DocumentController,
        ReadOnlyAuditor,
    ];
}