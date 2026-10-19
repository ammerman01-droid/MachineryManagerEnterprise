using System.Net.Http.Headers;
using MachineryManagerEnterprise.Identity.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Http;

/// <summary>
/// Attaches the current user's access token as a Bearer header on outgoing internal API calls.
/// The handler holds no per-user state, because HttpClientFactory reuses handler instances
/// across circuits and users.
/// </summary>
public sealed class BearerTokenHandler : DelegatingHandler
{
    private readonly CircuitServicesAccessor _circuitServicesAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>Initializes a new instance of the <see cref="BearerTokenHandler"/> class.</summary>
    /// <param name="circuitServicesAccessor">Gives access to the executing circuit's scope.</param>
    /// <param name="httpContextAccessor">Used during prerender, when no circuit exists yet.</param>
    public BearerTokenHandler(CircuitServicesAccessor circuitServicesAccessor, IHttpContextAccessor httpContextAccessor)
    {
        _circuitServicesAccessor = circuitServicesAccessor;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // The circuit's scope when inside a circuit; the request's scope during prerender.
        var services = _circuitServicesAccessor.Services ?? _httpContextAccessor.HttpContext?.RequestServices;
        var tokenAccessor = services?.GetService<ICurrentAccessTokenAccessor>();
        var accessToken = tokenAccessor is null ? null : await tokenAccessor.GetAccessTokenAsync();

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}