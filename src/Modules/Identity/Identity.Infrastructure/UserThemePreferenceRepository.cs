using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace MachineryManagerEnterprise.Identity.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IUserThemePreferenceRepository"/>,
/// storing the preference directly on the user's <see cref="ApplicationUser"/>
/// record via <see cref="UserManager{TUser}"/>.
/// </summary>
/// <remarks>
/// <see cref="UserManager{TUser}"/>'s methods do not accept a
/// <see cref="CancellationToken"/>, so the token on this repository's
/// methods is accepted for interface consistency but not currently passed
/// through anywhere.
/// </remarks>
internal sealed class UserThemePreferenceRepository : IUserThemePreferenceRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Creates the repository.
    /// </summary>
    /// <param name="userManager">Used to load and update the user's record.</param>
    public UserThemePreferenceRepository(UserManager<ApplicationUser> userManager) =>
        _userManager = userManager;

    /// <inheritdoc />
    public async Task<ThemePreference?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.ThemeMode is null || user.ThemeCornerStyle is null)
        {
            return null;
        }

        return new ThemePreference(user.ThemeMode, user.ThemeCornerStyle);
    }

    /// <inheritdoc />
    public async Task SetAsync(Guid userId, ThemePreference preference, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return;
        }

        user.ThemeMode = preference.Mode;
        user.ThemeCornerStyle = preference.CornerStyle;

        await _userManager.UpdateAsync(user);
    }
}