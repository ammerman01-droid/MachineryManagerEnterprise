using MachineryManagerEnterprise.Administration.Application.Features.Users.Dtos;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.SearchUsers;

/// <summary>
/// Handles <see cref="SearchUsersQuery"/>, returning only users the caller may see: everyone for
/// Platform, otherwise the users living under the caller's own Holding (or Organization).
/// </summary>
public sealed class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, Result<UserSearchResponse>>
{
    private readonly IUserAuthorizationService _authorization;
    private readonly IUserHomeRepository _homes;
    private readonly IUserAccountService _accounts;

    /// <summary>Initializes a new instance of the <see cref="SearchUsersQueryHandler"/> class.</summary>
    /// <param name="authorization">Decides which users the caller may see.</param>
    /// <param name="homes">Finds the users under a Holding/Organization.</param>
    /// <param name="accounts">Searches the accounts.</param>
    public SearchUsersQueryHandler(
        IUserAuthorizationService authorization,
        IUserHomeRepository homes,
        IUserAccountService accounts)
    {
        _authorization = authorization;
        _homes = homes;
        _accounts = accounts;
    }

    /// <inheritdoc />
    public async Task<Result<UserSearchResponse>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var visibility = await _authorization.ResolveUserVisibilityAsync(cancellationToken);

        if (visibility.IsFailure)
        {
            return Result.Failure<UserSearchResponse>(visibility.Error);
        }

        IReadOnlyCollection<Guid>? userIds = null;

        if (!visibility.Value.All)
        {
            userIds = await _homes.GetUserIdsAsync(
                visibility.Value.HoldingId,
                visibility.Value.OrganizationId,
                cancellationToken);
        }

        var page = await _accounts.SearchAsync(
            new UserAccountQuery(request.Search, userIds, request.Page, request.PageSize),
            cancellationToken);

        var totalPages = page.TotalItems == 0 ? 0 : (int)Math.Ceiling(page.TotalItems / (double)request.PageSize);

        return new UserSearchResponse(
            page.Items.Select(account => new UserDto(account.Id, account.UserName, account.IsActive)).ToList(),
            request.Page,
            request.PageSize,
            page.TotalItems,
            totalPages,
            request.Page < totalPages,
            request.Page > 1);
    }
}
