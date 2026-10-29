using MachineryManagerEnterprise.Administration.Application.Features.Users.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.SearchUsers;

/// <summary>Searches the users the caller may see.</summary>
/// <param name="Search">Optional user-name filter.</param>
/// <param name="Page">One-based page number.</param>
/// <param name="PageSize">Page size.</param>
public sealed record SearchUsersQuery(string? Search = null, int Page = 1, int PageSize = 25)
    : IRequest<Result<UserSearchResponse>>;
