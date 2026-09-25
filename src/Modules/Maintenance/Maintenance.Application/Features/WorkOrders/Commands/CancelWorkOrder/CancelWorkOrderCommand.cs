using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.CancelWorkOrder;

/// <summary>Command to cancel an Open Work Order, with a required reason.</summary>
public sealed record CancelWorkOrderCommand(Guid WorkOrderId, string Reason) : IRequest<Result>;
