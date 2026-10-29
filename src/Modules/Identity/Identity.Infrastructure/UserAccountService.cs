using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Identity.Infrastructure;

/// <summary>
/// ASP.NET Core Identity-backed implementation of <see cref="IUserAccountService"/>.
/// Pure account operations: authorization and tenant ownership are decided by the caller.
/// </summary>
public sealed class UserAccountService : IUserAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>Initializes a new instance of the <see cref="UserAccountService"/> class.</summary>
    /// <param name="userManager">The ASP.NET Core Identity user manager.</param>
    public UserAccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<Result> CreateAsync(
        Guid userId,
        string userName,
        string password,
        IReadOnlyCollection<string>? roles,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser { Id = userId, UserName = userName };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return Result.Failure(Error.Validation(
                "User.CreateFailed",
                string.Join("; ", result.Errors.Select(e => e.Description))));
        }

        if (roles is { Count: > 0 })
        {
            var validRoles = roles.Where(role => StandardRoles.All.Contains(role)).ToArray();

            if (validRoles.Length > 0)
            {
                await _userManager.AddToRolesAsync(user, validRoles);
            }
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<UserAccount?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user is null ? null : ToAccount(user);
    }

    /// <inheritdoc />
    public async Task<UserAccountPage> SearchAsync(
        UserAccountQuery query,
        CancellationToken cancellationToken = default)
    {
        var users = _userManager.Users.AsNoTracking();

        if (query.UserIds is not null)
        {
            var ids = query.UserIds.ToList();
            users = users.Where(user => ids.Contains(user.Id));
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search;
            users = users.Where(user => user.UserName != null && user.UserName.Contains(term));
        }

        var totalItems = await users.CountAsync(cancellationToken);

        var page = await users
            .OrderBy(user => user.UserName)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new UserAccountPage(
            page.Select(ToAccount).ToList(),
            query.Page,
            query.PageSize,
            totalItems);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user is null ? [] : (await _userManager.GetRolesAsync(user)).ToList();
    }

    /// <inheritdoc />
    public async Task<Result> SetActiveAsync(Guid userId, bool active, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.Failure(Error.NotFound("User.NotFound", $"User with id {userId} was not found."));
        }

        // Users created before the Lockout policy was added may still have
        // LockoutEnabled=false, which makes SetLockoutEndDateAsync reject the call.
        // Activation/deactivation is an explicit administrative action, independent of
        // the failed-login lockout feature, so it must always be possible.
        if (!user.LockoutEnabled)
        {
            var enableResult = await _userManager.SetLockoutEnabledAsync(user, true);

            if (!enableResult.Succeeded)
            {
                return Result.Failure(Error.Validation(
                    active ? "User.ActivateFailed" : "User.DeactivateFailed",
                    string.Join("; ", enableResult.Errors.Select(e => e.Description))));
            }
        }

        var result = await _userManager.SetLockoutEndDateAsync(
            user,
            active ? null : DateTimeOffset.UtcNow.AddYears(100));

        if (!result.Succeeded)
        {
            return Result.Failure(Error.Validation(
                active ? "User.ActivateFailed" : "User.DeactivateFailed",
                string.Join("; ", result.Errors.Select(e => e.Description))));
        }

        if (active)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is not null)
        {
            await _userManager.DeleteAsync(user);
        }
    }

    private static UserAccount ToAccount(ApplicationUser user) =>
        new(
            user.Id,
            user.UserName ?? string.Empty,
            !user.LockoutEnabled || user.LockoutEnd is null || user.LockoutEnd <= DateTimeOffset.UtcNow);
}
