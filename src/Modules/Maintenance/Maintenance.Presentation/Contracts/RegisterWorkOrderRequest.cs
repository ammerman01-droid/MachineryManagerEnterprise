using MachineryManagerEnterprise.Maintenance.Domain;

namespace MachineryManagerEnterprise.Maintenance.Presentation.Contracts;

/// <summary>HTTP request body for registering a new Work Order.</summary>
public sealed record RegisterWorkOrderRequest(
    Guid OrganizationId,
    Guid AssetId,
    DateTimeOffset ReportedAt,
    WorkOrderAssetStatus ResultingAssetStatus,
    decimal MeterReading,
    RepairPriority Priority,
    RepairType RepairType,
    Guid ResponsiblePersonnelId,
    string ObservationDescription,
    RepairLocation PredictedRepairLocation,
    RepairablePart PartNeedingRepair);
