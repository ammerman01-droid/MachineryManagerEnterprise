using MachineryManagerEnterprise.Maintenance.Domain;

namespace MachineryManagerEnterprise.Maintenance.Presentation.Contracts;

/// <summary>HTTP request body for editing an Open Work Order.</summary>
public sealed record EditWorkOrderRequest(
    DateTimeOffset ReportedAt,
    WorkOrderAssetStatus ResultingAssetStatus,
    decimal MeterReading,
    RepairPriority Priority,
    RepairType RepairType,
    Guid ResponsiblePersonnelId,
    string ObservationDescription,
    RepairLocation PredictedRepairLocation,
    RepairablePart PartNeedingRepair);
