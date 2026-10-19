using System.Globalization;
using System.Net;
using System.Text.Json;
using MachineryManagerEnterprise.Identity.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityClientOptions = MachineryManagerEnterprise.Identity.Infrastructure.Options.OpenIddictClientOptions;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Http;

/// <inheritdoc cref="ICurrentAccessTokenAccessor" />
/// <remarks>
/// Registered as Scoped, so each Blazor circuit (and each prerender request) owns its
/// own copy. Tokens are read from the sign-in cookie during prerender and carried into
/// the interactive circuit through <see cref="PersistentComponentState"/> (which is
/// encrypted by Data Protection). When the access token is about to expire it is
/// refreshed in memory against /connect/token; the refresh token is non-rolling on the
/// server, so the cookie's copy stays valid for new circuits.
/// </remarks>
public sealed class CurrentAccessTokenAccessor : ICurrentAccessTokenAccessor, IDisposable
{
    /// <summary>Name of the HttpClient (no bearer handler) used to call the token endpoint.</summary>
    internal const string TokenEndpointClientName = "IdentityTokenEndpoint";

    private const string PersistenceKey = "MachineryManagerEnterprise.Identity.SessionTokens";
    private static readonly TimeSpan RefreshSkew = TimeSpan.FromMinutes(2);

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PersistentComponentState _persistentState;
    private readonly PersistingComponentStateSubscription _persistingSubscription;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<IdentityClientOptions> _clientOptions;
    private readonly ILogger<CurrentAccessTokenAccessor> _logger;
    private readonly SemaphoreSlim _gate = new(1, 1);

    private SessionTokens? _tokens;
    private bool _hasCached;

    /// <summary>Initializes a new instance of the <see cref="CurrentAccessTokenAccessor"/> class.</summary>
    /// <param name="httpContextAccessor">Provides the request's HttpContext when one is available.</param>
    /// <param name="persistentState">Carries the tokens captured during prerender into the circuit.</param>
    /// <param name="httpClientFactory">Creates the client used to call the token endpoint.</param>
    /// <param name="clientOptions">Provides the Web client's id and secret.</param>
    /// <param name="logger">Diagnostic logger.</param>
    public CurrentAccessTokenAccessor(
        IHttpContextAccessor httpContextAccessor,
        PersistentComponentState persistentState,
        IHttpClientFactory httpClientFactory,
        IOptions<IdentityClientOptions> clientOptions,
        ILogger<CurrentAccessTokenAccessor> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _persistentState = persistentState;
        _httpClientFactory = httpClientFactory;
        _clientOptions = clientOptions;
        _logger = logger;
        _persistingSubscription = _persistentState.RegisterOnPersisting(PersistTokensAsync);
    }

    /// <inheritdoc />
    public async Task<string?> GetAccessTokenAsync()
    {
        await _gate.WaitAsync();
        try
        {
            if (!_hasCached)
            {
                await ResolveTokensAsync();
            }

            if (NeedsRefresh())
            {
                await RefreshAsync();
            }

            return _tokens?.AccessToken;
        }
        finally
        {
            _gate.Release();
        }
    }

    private bool NeedsRefresh() =>
        _tokens is { RefreshToken: { Length: > 0 }, ExpiresAt: { } expiresAt }
        && expiresAt - DateTimeOffset.UtcNow <= RefreshSkew;

    private async Task ResolveTokensAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            var accessToken = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "access_token");
            var refreshToken = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "refresh_token");
            var expiresRaw = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "expires_at");

            DateTimeOffset? expiresAt = DateTimeOffset.TryParse(
                expiresRaw,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out var parsed)
                ? parsed
                : null;

            _tokens = new SessionTokens(accessToken, refreshToken, expiresAt);
        }
        else if (_persistentState.TryTakeFromJson<SessionTokens>(PersistenceKey, out var restored))
        {
            _tokens = restored;
        }

        _hasCached = true;
    }

    private async Task RefreshAsync()
    {
        var current = _tokens!;
        var web = _clientOptions.Value.Web;

        try
        {
            using var client = _httpClientFactory.CreateClient(TokenEndpointClientName);
            using var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = current.RefreshToken!,
                ["client_id"] = web.ClientId,
                ["client_secret"] = web.ClientSecret ?? string.Empty,
            });

            using var response = await client.PostAsync("connect/token", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Access token refresh was rejected with HTTP {StatusCode}.", (int)response.StatusCode);

                // The refresh token itself is invalid (revoked, expired): drop the session tokens.
                if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized)
                {
                    _tokens = new SessionTokens(null, null, null);
                }

                return;
            }

            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = document.RootElement;

            var accessToken = root.GetProperty("access_token").GetString();
            var expiresIn = root.TryGetProperty("expires_in", out var expiresInElement) ? expiresInElement.GetInt32() : 600;
            var refreshToken = root.TryGetProperty("refresh_token", out var refreshElement)
                ? refreshElement.GetString()
                : current.RefreshToken;

            _tokens = new SessionTokens(accessToken, refreshToken, DateTimeOffset.UtcNow.AddSeconds(expiresIn));
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException or KeyNotFoundException)
        {
            // Transient failure: keep the current token (still valid for a short while) and retry on the next call.
            _logger.LogWarning(ex, "Access token refresh failed.");
        }
    }

    private async Task PersistTokensAsync()
    {
        await GetAccessTokenAsync();
        _persistentState.PersistAsJson(PersistenceKey, _tokens);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _persistingSubscription.Dispose();
        _gate.Dispose();
    }

    /// <summary>The token triple held for one circuit/request.</summary>
    internal sealed record SessionTokens(string? AccessToken, string? RefreshToken, DateTimeOffset? ExpiresAt);
}