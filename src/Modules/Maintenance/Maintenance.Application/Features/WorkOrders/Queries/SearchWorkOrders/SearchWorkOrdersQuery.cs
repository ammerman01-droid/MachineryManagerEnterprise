using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.SearchWorkOrders;

/// <summary>
/// Query to search Work Orders within an Organization.
/// <see cref="SearchTerm"/>, when provided, matches against the Work
/// Order's number and observation description (chat, 2026-09-22).
/// </summary>
public sealed record SearchWorkOrdersQuery(Guid OrganizationId, string? SearchTerm, int Page = 1, int PageSize = 20)
    : IRequest<Result<SearchWorkOrdersResponse>>;

/// <summary>Paginated response for <see cref="SearchWorkOrdersQuery"/>.</summary>
public sealed record SearchWorkOrdersResponse(
    IReadOnlyCollection<WorkOrderDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);
