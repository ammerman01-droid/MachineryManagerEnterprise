using MachineryManager.Identity.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MachineryManager.Identity.Infrastructure.Http;

/// <inheritdoc cref="ICurrentAccessTokenAccessor" />
/// <remarks>
/// Registered as Scoped: in Blazor Server, a scoped service's lifetime
/// matches the user's circuit, so the cached token is naturally
/// isolated per user/session and cleared when the circuit ends.
///
/// IMPORTANT (chat, 2026-08-24): IHttpContextAccessor.HttpContext is
/// only reliable during the initial prerender pass of a request — once
/// the SignalR circuit for an InteractiveServer component connects, a
/// NEW DI scope is created and HttpContext is null/unreliable there
/// (this is documented ASP.NET Core behavior, not a bug in this app).
/// To bridge the two passes, the token read during prerender (when
/// HttpContext IS valid) is captured into PersistentComponentState —
/// the framework's own mechanism for carrying prerendered state into
/// the interactive circuit — and recovered from there when HttpContext
/// is unavailable.
/// </remarks>
public sealed class CurrentAccessTokenAccessor : ICurrentAccessTokenAccessor, IDisposable
{
    private const string PersistenceKey = "MachineryManager.Identity.AccessToken";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PersistentComponentState _persistentState;
    private readonly PersistingComponentStateSubscription _persistingSubscription;

    private string? _cachedToken;
    private bool _hasCached;

    /// <summary>Initializes a new instance of the <see cref="CurrentAccessTokenAccessor"/> class.</summary>
    /// <param name="httpContextAccessor">Provides access to the current request's HttpContext, when available.</param>
    /// <param name="persistentState">Carries the token captured during prerender into the interactive circuit.</param>
    public CurrentAccessTokenAccessor(
        IHttpContextAccessor httpContextAccessor,
        PersistentComponentState persistentState)
    {
        _httpContextAccessor = httpContextAccessor;
        _persistentState = persistentState;
        _persistingSubscription = _persistentState.RegisterOnPersisting(PersistTokenAsync);
    }

    /// <inheritdoc />
    public async Task<string?> GetAccessTokenAsync()
    {
        if (_hasCached)
        {
            return _cachedToken;
        }

        await ResolveTokenAsync();
        return _cachedToken;
    }

    private async Task ResolveTokenAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            // Prerender pass (or a plain HTTP request outside a Blazor
            // circuit): HttpContext is real here, read the token
            // directly from the sign-in cookie's stored tokens.
            _cachedToken = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "access_token");
        }
        else if (_persistentState.TryTakeFromJson<string>(PersistenceKey, out var restoredToken))
        {
            // Interactive circuit pass: recover the value captured
            // during this same request's prerender pass.
            _cachedToken = restoredToken;
        }

        _hasCached = true;
    }

    private async Task PersistTokenAsync()
    {
        // Ensures the token is captured even if nothing called
        // GetAccessTokenAsync() earlier in this same prerender pass —
        // makes persistence self-contained, independent of component
        // render order.
        if (!_hasCached)
        {
            await ResolveTokenAsync();
        }

        _persistentState.PersistAsJson(PersistenceKey, _cachedToken);
    }

    /// <inheritdoc />
    public void Dispose() => _persistingSubscription.Dispose();
}