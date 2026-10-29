using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Account-level operations on application users, implemented by the Identity module and
/// consumed by the Administration module (which owns authorization rules, assignments and
/// each user's position in the Holding/Organization/Project tree — per ADR-0030 Identity
/// never stores Organization data).
/// </summary>
public interface IUserAccountService
{
    /// <summary>Creates an account with a caller-chosen identifier.</summary>
    /// <param name="userId">The identifier to give the new account.</param>
    /// <param name="userName">The sign-in name.</param>
    /// <param name="password">The initial password.</param>
    /// <param name="roles">Identity roles to grant (unknown roles are ignored), or <see langword="null"/>.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Success, or a validation error (duplicate name, weak password, ...).</returns>
    Task<Result> CreateAsync(
        Guid userId,
        string userName,
        string password,
        IReadOnlyCollection<string>? roles,
        CancellationToken cancellationToken = default);

    /// <summary>Finds an account by identifier.</summary>
    /// <param name="userId">The account identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The account, or <see langword="null"/> when it does not exist.</returns>
    Task<UserAccount?> GetAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Searches accounts, optionally restricted to a set of identifiers.</summary>
    /// <param name="query">The search criteria.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>One page of accounts.</returns>
    Task<UserAccountPage> SearchAsync(UserAccountQuery query, CancellationToken cancellationToken = default);

    /// <summary>Returns the Identity role names of an account.</summary>
    /// <param name="userId">The account identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The role names (empty when none or unknown account).</returns>
    Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Locks out (<c>false</c>) or re-enables (<c>true</c>) an account.</summary>
    /// <param name="userId">The account identifier.</param>
    /// <param name="active">The desired state.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Success, or an error.</returns>
    Task<Result> SetActiveAsync(Guid userId, bool active, CancellationToken cancellationToken = default);

    /// <summary>Deletes an account. Used only to roll back a failed multi-step creation.</summary>
    /// <param name="userId">The account identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>An application user account.</summary>
/// <param name="Id">The account identifier.</param>
/// <param name="UserName">The sign-in name.</param>
/// <param name="IsActive">Whether the account can currently sign in.</param>
public sealed record UserAccount(Guid Id, string UserName, bool IsActive);

/// <summary>Search criteria for <see cref="IUserAccountService.SearchAsync"/>.</summary>
/// <param name="Search">Optional user-name filter.</param>
/// <param name="UserIds">
/// When not <see langword="null"/>, only these accounts are considered (an empty collection
/// matches nothing); <see langword="null"/> means every account.
/// </param>
/// <param name="Page">One-based page number.</param>
/// <param name="PageSize">Page size.</param>
public sealed record UserAccountQuery(
    string? Search,
    IReadOnlyCollection<Guid>? UserIds,
    int Page,
    int PageSize);

/// <summary>One page of accounts.</summary>
/// <param name="Items">The accounts on the page.</param>
/// <param name="Page">The page number.</param>
/// <param name="PageSize">The page size.</param>
/// <param name="TotalItems">The total number of matching accounts.</param>
public sealed record UserAccountPage(
    IReadOnlyList<UserAccount> Items,
    int Page,
    int PageSize,
    int TotalItems);
