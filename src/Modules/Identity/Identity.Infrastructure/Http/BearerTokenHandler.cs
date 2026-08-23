using System.Net.Http.Headers;
using MachineryManager.Identity.Domain;

namespace MachineryManager.Identity.Infrastructure.Http;

/// <summary>
/// Attaches the current user's cached access token (via
/// <see cref="ICurrentAccessTokenAccessor"/>) as a Bearer
/// Authorization header on outgoing internal API calls
/// (chat, 2026-08-22).
/// </summary>
public sealed class BearerTokenHandler : DelegatingHandler
{
    private readonly ICurrentAccessTokenAccessor _tokenAccessor;

    /// <summary>Initializes a new instance of the <see cref="BearerTokenHandler"/> class.</summary>
    /// <param name="tokenAccessor">Provides the current user's cached access token.</param>
    public BearerTokenHandler(ICurrentAccessTokenAccessor tokenAccessor)
    {
        _tokenAccessor = tokenAccessor;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await _tokenAccessor.GetAccessTokenAsync();

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}