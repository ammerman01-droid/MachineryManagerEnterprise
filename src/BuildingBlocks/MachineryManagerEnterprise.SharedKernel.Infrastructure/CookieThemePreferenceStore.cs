using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MachineryManagerEnterprise.SharedKernel.Infrastructure;

/// <summary>
/// Cookie-backed implementation of <see cref="IThemePreferenceStore"/>.
/// Reads the preference cookie set on a previous request/response; writing
/// from the server is only attempted when the HTTP response has not yet
/// started, since an active Blazor Server circuit's response has already
/// completed by the time the user changes the theme (that path instead
/// writes the cookie via JS interop — see <c>ThemeService</c> in
/// <c>MachineryManagerEnterprise.UI</c>). Registered as the default
/// implementation by <c>AddSharedKernelInfrastructure</c>; the Identity
/// module overrides this registration with <see cref="CompositeThemePreferenceStore"/>
/// for authenticated users.
/// </summary>
internal sealed class CookieThemePreferenceStore : IThemePreferenceStore
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Creates the store.
    /// </summary>
    /// <param name="httpContextAccessor">Gives access to the current request's cookies and response.</param>
    public CookieThemePreferenceStore(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    /// <inheritdoc />
    public Task<ThemePreference?> ReadAsync(CancellationToken cancellationToken = default)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is null || !context.Request.Cookies.TryGetValue(ThemeCookieCodec.CookieName, out var raw))
        {
            return Task.FromResult<ThemePreference?>(null);
        }

        return Task.FromResult(ThemeCookieCodec.Parse(raw));
    }

    /// <inheritdoc />
    public Task WriteAsync(ThemePreference preference, CancellationToken cancellationToken = default)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is not null && !context.Response.HasStarted)
        {
            context.Response.Cookies.Append(
                ThemeCookieCodec.CookieName,
                ThemeCookieCodec.Format(preference),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });
        }

        return Task.CompletedTask;
    }
}