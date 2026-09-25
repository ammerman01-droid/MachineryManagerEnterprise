using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.SearchWorkOrders;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Maintenance.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::MachineryManagerEnterprise.Maintenance.Domain.WorkOrder"/> aggregate.</summary>
public interface IWorkOrderRepository : IRepository<
    global::MachineryManagerEnterprise.Maintenance.Domain.WorkOrder,
    global::MachineryManagerEnterprise.Maintenance.Domain.WorkOrderId>
{
    /// <summary>Performs a paginated search over Work Orders within the given Organization.</summary>
    Task<SearchWorkOrdersResponse> SearchAsync(
        Guid organizationId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
