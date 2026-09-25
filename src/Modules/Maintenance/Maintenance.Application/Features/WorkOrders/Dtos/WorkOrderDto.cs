namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Dtos;

/// <summary>
/// Read-only projection of a Work Order for API/UI consumption.
/// <see cref="AssetCode"/>, <see cref="AssetName"/>, and
/// <see cref="ResponsiblePersonnelName"/> are enriched by the Query
/// handler via cross-module lookups (IAssetLookupService,
/// IPersonnelLookupService) after the base Mapster projection — they
/// are display-only and not part of the WorkOrder aggregate itself.
/// </summary>
public sealed record WorkOrderDto(
    Guid Id,
    Guid OrganizationId,
    Guid AssetId,
    Guid ProjectId,
    int Number,
    DateTimeOffset ReportedAt,
    string ResultingAssetStatus,
    decimal MeterReading,
    string Priority,
    string RepairType,
    Guid ResponsiblePersonnelId,
    string ObservationDescription,
    string PredictedRepairLocation,
    string PartNeedingRepair,
    string Status,
    string? CancellationReason,
    string? AssetCode = null,
    string? AssetName = null,
    string? ResponsiblePersonnelName = null);
