namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Reads and writes the current user's UI theme preference. Concrete
/// storage mechanism (cookie, the authenticated user's database record, or
/// a combination) is chosen by the implementation registered in
/// Infrastructure.
/// </summary>
public interface IThemePreferenceStore
{
    /// <summary>
    /// Reads the preference available for the current request/user, or
    /// <see langword="null"/> if none is set yet.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the read.</param>
    Task<ThemePreference?> ReadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the given preference for future requests.
    /// </summary>
    /// <param name="preference">The theme preference to persist.</param>
    /// <param name="cancellationToken">Token to cancel the write.</param>
    Task WriteAsync(ThemePreference preference, CancellationToken cancellationToken = default);
}

/// <summary>
/// The user's chosen theme settings, as the raw string values of
/// <c>AppThemeMode</c> and <c>AppCornerStyle</c> (defined in
/// <c>MachineryManagerEnterprise.UI</c>, which this project does not
/// reference — hence plain strings here rather than the enums themselves).
/// </summary>
/// <param name="Mode">The name of the selected <c>AppThemeMode</c> value.</param>
/// <param name="CornerStyle">The name of the selected <c>AppCornerStyle</c> value.</param>
public readonly record struct ThemePreference(string Mode, string CornerStyle);