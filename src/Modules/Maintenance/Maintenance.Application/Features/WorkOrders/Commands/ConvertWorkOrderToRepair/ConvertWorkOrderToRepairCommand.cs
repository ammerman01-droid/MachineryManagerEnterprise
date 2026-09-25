using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.ConvertWorkOrderToRepair;

/// <summary>
/// Command to convert an Open Work Order to repair. No approval is
/// required (chat, 2026-09-22) — any user holding the ordinary
/// "WorkOrder.Edit" permission over the Work Order's scope may perform
/// this transition.
/// </summary>
public sealed record ConvertWorkOrderToRepairCommand(Guid WorkOrderId) : IRequest<Result>;
