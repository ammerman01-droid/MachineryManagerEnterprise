using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MachineryManagerEnterprise.SharedKernel.Infrastructure;

/// <summary>
/// Theme preference store that prefers the authenticated user's saved
/// preference (via <see cref="IUserThemePreferenceRepository"/>, persisted
/// per-user so it follows them across browsers and devices) and falls back
/// to the preference cookie for anonymous users, or when the authenticated
/// user has never saved one yet. Registered by the Identity module (see
/// <c>AddIdentityInfrastructure</c>) so it takes over from the
/// cookie-only <see cref="CookieThemePreferenceStore"/> registered by
/// <c>AddSharedKernelInfrastructure</c> — the last registration of
/// <see cref="IThemePreferenceStore"/> in the DI container wins.
/// </summary>
public sealed class CompositeThemePreferenceStore : IThemePreferenceStore
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserThemePreferenceRepository _userThemePreferenceRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Creates the store.
    /// </summary>
    /// <param name="currentUserService">Used to check whether the current request is authenticated, and by whom.</param>
    /// <param name="userThemePreferenceRepository">Reads/writes the preference saved on the authenticated user's record.</param>
    /// <param name="httpContextAccessor">Gives access to the current request's cookies and response, for the anonymous/fallback path.</param>
    public CompositeThemePreferenceStore(
        ICurrentUserService currentUserService,
        IUserThemePreferenceRepository userThemePreferenceRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _currentUserService = currentUserService;
        _userThemePreferenceRepository = userThemePreferenceRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public async Task<ThemePreference?> ReadAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserService is { IsAuthenticated: true, UserId: { } userId })
        {
            var saved = await _userThemePreferenceRepository.GetAsync(userId, cancellationToken);
            if (saved is not null)
            {
                return saved;
            }
        }

        return ReadCookie();
    }

    /// <inheritdoc />
    public async Task WriteAsync(ThemePreference preference, CancellationToken cancellationToken = default)
    {
        if (_currentUserService is { IsAuthenticated: true, UserId: { } userId })
        {
            await _userThemePreferenceRepository.SetAsync(userId, preference, cancellationToken);
        }

        WriteCookieIfPossible(preference);
    }

    /// <summary>
    /// Reads the preference from the theme cookie — used for anonymous
    /// users, and as the fallback before an authenticated user has ever
    /// saved a preference.
    /// </summary>
    private ThemePreference? ReadCookie()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is null || !context.Request.Cookies.TryGetValue(ThemeCookieCodec.CookieName, out var raw))
        {
            return null;
        }

        return ThemeCookieCodec.Parse(raw);
    }

    /// <summary>
    /// Writes the preference cookie directly from the server, when the HTTP
    /// response has not started yet. During an active Blazor Server
    /// circuit this is always a no-op (the response has already completed
    /// by then) — <c>ThemeService</c> writes the cookie from the client via
    /// JS interop in that case instead.
    /// </summary>
    /// <param name="preference">The preference to write to the cookie.</param>
    private void WriteCookieIfPossible(ThemePreference preference)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is not null && !context.Response.HasStarted)
        {
            context.Response.Cookies.Append(
                ThemeCookieCodec.CookieName,
                ThemeCookieCodec.Format(preference),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });
        }
    }
}