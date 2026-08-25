using MachineryManager.Identity.Domain;
using Microsoft.AspNetCore.Identity;

namespace MachineryManager.Identity.Infrastructure.Validation;

/// <summary>
/// Enforces the maximum username length specified by the product
/// owner (<see cref="UsernamePolicy.MaxLength"/>). ASP.NET Core
/// Identity has no built-in maximum-length option for usernames — the
/// allowed-characters check is already covered natively via
/// <c>IdentityOptions.User.AllowedUserNameCharacters</c>.
/// </summary>
public sealed class UsernameLengthValidator : IUserValidator<ApplicationUser>
{
    /// <inheritdoc />
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        if (!string.IsNullOrEmpty(user.UserName) && user.UserName.Length > UsernamePolicy.MaxLength)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "UserNameTooLong",
                Description = $"Username must not exceed {UsernamePolicy.MaxLength} characters.",
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}