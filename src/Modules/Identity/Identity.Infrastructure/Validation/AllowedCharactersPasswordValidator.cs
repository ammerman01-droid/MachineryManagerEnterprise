using MachineryManager.Identity.Domain;
using Microsoft.AspNetCore.Identity;

namespace MachineryManager.Identity.Infrastructure.Validation;

/// <summary>
/// Enforces the password character-set restriction specified by the
/// product owner (<see cref="PasswordPolicy.AllowedCharacters"/>):
/// English letters, digits, and standard ASCII special characters
/// only. ASP.NET Core Identity has no built-in charset restriction for
/// passwords.
/// </summary>
public sealed class AllowedCharactersPasswordValidator : IPasswordValidator<ApplicationUser>
{
    /// <inheritdoc />
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
    {
        if (password is not null && !PasswordPolicy.AllowedCharacters.IsMatch(password))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordInvalidCharacters",
                Description = "Password may only contain English letters, digits, and standard special characters.",
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}