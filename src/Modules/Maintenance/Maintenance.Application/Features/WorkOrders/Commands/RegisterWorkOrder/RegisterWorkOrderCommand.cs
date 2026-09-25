using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.RegisterWorkOrder;

/// <summary>
/// Command to register a new Work Order for an Asset within an
/// Organization (chat, 2026-09-22). The Project is deliberately NOT a
/// parameter here — it is read from the Asset itself by the handler,
/// per the confirmed business rule.
/// </summary>
public sealed record RegisterWorkOrderCommand(
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
    RepairablePart PartNeedingRepair) : IRequest<Result<Guid>>;
