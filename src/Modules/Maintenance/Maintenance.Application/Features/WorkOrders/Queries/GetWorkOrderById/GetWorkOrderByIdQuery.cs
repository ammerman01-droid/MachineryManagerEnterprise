using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.GetWorkOrderById;

/// <summary>Query to retrieve a single Work Order by its identifier.</summary>
public sealed record GetWorkOrderByIdQuery(Guid WorkOrderId) : IRequest<Result<WorkOrderDto>>;
