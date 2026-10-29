namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Dtos;

/// <summary>Read model for a user account.</summary>
/// <param name="Id">The user identifier.</param>
/// <param name="UserName">The sign-in name.</param>
/// <param name="IsActive">Whether the account can currently sign in.</param>
public sealed record UserDto(Guid Id, string UserName, bool IsActive);

/// <summary>One page of users.</summary>
/// <param name="Items">The users on the page.</param>
/// <param name="Page">The page number.</param>
/// <param name="PageSize">The page size.</param>
/// <param name="TotalItems">The total number of matching users.</param>
/// <param name="TotalPages">The total number of pages.</param>
/// <param name="HasNextPage">Whether a next page exists.</param>
/// <param name="HasPreviousPage">Whether a previous page exists.</param>
public sealed record UserSearchResponse(
    IReadOnlyList<UserDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);
