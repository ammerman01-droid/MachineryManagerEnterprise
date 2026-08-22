using MachineryManager.Administration.Application.Features.Profiles.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Queries.SearchProfiles;

/// <summary>Query to perform a paginated search over profiles.</summary>
public sealed record SearchProfilesQuery(
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 25)
    : IRequest<Result<SearchProfilesResponse>>;

/// <summary>Paginated response for profile search queries.</summary>
public sealed record SearchProfilesResponse(
    IReadOnlyList<ProfileDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);