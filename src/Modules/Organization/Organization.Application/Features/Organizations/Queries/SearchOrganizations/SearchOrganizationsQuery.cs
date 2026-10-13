using MachineryManagerEnterprise.Organization.Application.Features.Organizations.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Organization.Application.Features.Organizations.Queries.SearchOrganizations;

/// <summary>
/// Query to perform a paginated search over organizations.
/// </summary>
/// <param name="SearchTerm">Optional text to filter by organization name.</param>
/// <param name="HoldingId">
/// Optional explicit filter restricting results to Organizations
/// belonging to this Holding (chat, 2026-10-04) — distinct from the
/// authorized-scope filtering already applied by the Handler: that
/// restricts results to what the user is *allowed* to see, while this
/// narrows within that to what the caller *asked* to see (e.g. a
/// cascading Holding → Organization picker in the UI).
/// </param>
/// <param name="Page">The one-based page number (default 1).</param>
/// <param name="PageSize">The number of items per page (default 25, max 200).</param>
public sealed record SearchOrganizationsQuery(
    string? SearchTerm = null,
    Guid? HoldingId = null,
    int Page = 1,
    int PageSize = 25)
    : IRequest<Result<SearchOrganizationsResponse>>;

/// <summary>
/// Paginated response for organization search queries.
/// </summary>
/// <param name="Items">The organizations matching the search criteria.</param>
/// <param name="Page">The current page number.</param>
/// <param name="PageSize">The number of items per page.</param>
/// <param name="TotalItems">The total count of matching items across all pages.</param>
/// <param name="TotalPages">The total number of pages.</param>
/// <param name="HasNextPage">Whether a next page exists.</param>
/// <param name="HasPreviousPage">Whether a previous page exists.</param>
public sealed record SearchOrganizationsResponse(
    IReadOnlyList<OrganizationDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);
