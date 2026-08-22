using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;

namespace MachineryManager.Identity.Infrastructure.Http;

/// <summary>
/// Attaches the current user's stored OpenIddict access token (obtained
/// via the Authorization Code + PKCE flow at /identity/connect) as a
/// Bearer Authorization header on outgoing internal API calls
/// (chat, 2026-08-22).
/// </summary>
public sealed class BearerTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>Initializes a new instance of the <see cref="BearerTokenHandler"/> class.</summary>
    /// <param name="httpContextAccessor">Provides access to the current request's HttpContext.</param>
    public BearerTokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            var accessToken = await httpContext.GetTokenAsync(
                IdentityConstants.ApplicationScheme,
                "access_token");

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}