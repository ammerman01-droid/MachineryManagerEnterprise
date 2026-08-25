using MachineryManager.Organization.Application.Features.Projects.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Projects.Queries.SearchProjects;

/// <summary>
/// Query to perform a paginated search over projects.
/// </summary>
/// <param name="SearchTerm">Optional text to filter by project name.</param>
/// <param name="Page">The one-based page number (default 1).</param>
/// <param name="PageSize">The number of items per page (default 25, max 200).</param>
public sealed record SearchProjectsQuery(
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 25)
    : IRequest<Result<SearchProjectsResponse>>;

/// <summary>
/// Paginated response for project search queries.
/// </summary>
/// <param name="Items">The projects matching the search criteria.</param>
/// <param name="Page">The current page number.</param>
/// <param name="PageSize">The number of items per page.</param>
/// <param name="TotalItems">The total count of matching items across all pages.</param>
/// <param name="TotalPages">The total number of pages.</param>
/// <param name="HasNextPage">Whether a next page exists.</param>
/// <param name="HasPreviousPage">Whether a previous page exists.</param>
public sealed record SearchProjectsResponse(
    IReadOnlyList<ProjectDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);