using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.EditWorkOrder;

/// <summary>
/// Command to edit an Open Work Order's details (chat, 2026-09-22). The
/// Asset, Project, and Work Order number are permanent and not
/// included here.
/// </summary>
public sealed record EditWorkOrderCommand(
    Guid WorkOrderId,
    DateTimeOffset ReportedAt,
    WorkOrderAssetStatus ResultingAssetStatus,
    decimal MeterReading,
    RepairPriority Priority,
    RepairType RepairType,
    Guid ResponsiblePersonnelId,
    string ObservationDescription,
    RepairLocation PredictedRepairLocation,
    RepairablePart PartNeedingRepair) : IRequest<Result>;
