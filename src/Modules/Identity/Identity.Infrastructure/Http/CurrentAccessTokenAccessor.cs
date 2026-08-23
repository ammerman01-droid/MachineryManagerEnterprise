using MachineryManager.Identity.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MachineryManager.Identity.Infrastructure.Http;

/// <inheritdoc cref="ICurrentAccessTokenAccessor" />
/// <remarks>
/// Registered as Scoped: in Blazor Server, a scoped service's lifetime
/// matches the user's circuit, so the cached token is naturally
/// isolated per user/session and cleared when the circuit ends.
/// </remarks>
public sealed class CurrentAccessTokenAccessor : ICurrentAccessTokenAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? _cachedToken;
    private bool _hasCached;

    /// <summary>Initializes a new instance of the <see cref="CurrentAccessTokenAccessor"/> class.</summary>
    /// <param name="httpContextAccessor">Provides access to the current request's HttpContext, when available.</param>
    public CurrentAccessTokenAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public async Task<string?> GetAccessTokenAsync()
    {
        if (_hasCached)
        {
            return _cachedToken;
        }

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            _cachedToken = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "access_token");
            _hasCached = true;
        }

        return _cachedToken;
    }
}