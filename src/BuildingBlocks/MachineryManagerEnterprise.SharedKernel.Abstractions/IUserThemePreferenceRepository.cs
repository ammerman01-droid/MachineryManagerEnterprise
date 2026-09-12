namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Reads and writes an authenticated user's theme preference, persisted on
/// that user's own record. Implemented in the Identity module's
/// Infrastructure layer, since <c>ApplicationUser</c> lives there; declared
/// here so <c>CompositeThemePreferenceStore</c> (in
/// SharedKernel.Infrastructure) can depend on it without a reverse
/// reference to the Identity module.
/// </summary>
public interface IUserThemePreferenceRepository
{
    /// <summary>
    /// Reads the given user's saved theme preference, or
    /// <see langword="null"/> if they have never saved one.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the read.</param>
    Task<ThemePreference?> GetAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the given theme preference on the user's record.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="preference">The theme preference to save.</param>
    /// <param name="cancellationToken">Token to cancel the write.</param>
    Task SetAsync(Guid userId, ThemePreference preference, CancellationToken cancellationToken = default);
}