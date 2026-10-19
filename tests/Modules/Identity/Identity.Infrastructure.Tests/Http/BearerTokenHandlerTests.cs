using System.Security.Claims;
using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests.Http;

/// <summary>
/// Verifies <see cref="BearerTokenHandler"/> attaches (or omits) the Bearer
/// Authorization header, resolving the token from the executing circuit's scope
/// when inside a Blazor circuit and from the request's cookie otherwise
/// (chat, 2026-08-22; reworked 2026-10-19 so a pooled handler never shares one
/// user's token with another).
/// </summary>
public sealed class BearerTokenHandlerTests
{
    private readonly CircuitServicesAccessor _circuitServicesAccessor = new();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly RecordingHandler _innerHandler = new();

    private HttpClient CreateClient()
    {
        var handler = new BearerTokenHandler(_circuitServicesAccessor, _httpContextAccessor)
        {
            InnerHandler = _innerHandler,
        };

        return new HttpClient(handler) { BaseAddress = new Uri("https://internal-api.local/") };
    }

    /// <summary>Builds a circuit scope whose token accessor returns <paramref name="token"/>.</summary>
    private static IServiceProvider CreateCircuitServices(string? token)
    {
        var tokenAccessor = Substitute.For<ICurrentAccessTokenAccessor>();
        tokenAccessor.GetAccessTokenAsync().Returns(token);

        return new ServiceCollection()
            .AddSingleton(tokenAccessor)
            .BuildServiceProvider();
    }

    /// <summary>Builds an HttpContext whose application-scheme cookie holds <paramref name="accessToken"/>.</summary>
    private static HttpContext CreateHttpContextWithToken(string accessToken)
    {
        var properties = new AuthenticationProperties();
        properties.StoreTokens([new AuthenticationToken { Name = "access_token", Value = accessToken }]);

        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity()),
            properties,
            IdentityConstants.ApplicationScheme);

        var authenticationService = Substitute.For<IAuthenticationService>();
        authenticationService
            .AuthenticateAsync(Arg.Any<HttpContext>(), IdentityConstants.ApplicationScheme)
            .Returns(AuthenticateResult.Success(ticket));

        return new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddSingleton(authenticationService)
                .BuildServiceProvider(),
        };
    }

    [Fact]
    public async Task SendAsync_InsideCircuit_WhenTokenAvailable_AttachesBearerAuthorizationHeader()
    {
        _circuitServicesAccessor.Services = CreateCircuitServices("test-access-token");

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest.Should().NotBeNull();
        _innerHandler.LastRequest!.Headers.Authorization.Should().NotBeNull();
        _innerHandler.LastRequest.Headers.Authorization!.Scheme.Should().Be("Bearer");
        _innerHandler.LastRequest.Headers.Authorization.Parameter.Should().Be("test-access-token");
    }

    [Fact]
    public async Task SendAsync_InsideCircuit_WhenTokenIsNull_DoesNotAttachAuthorizationHeader()
    {
        _circuitServicesAccessor.Services = CreateCircuitServices(null);

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
    }

    [Fact]
    public async Task SendAsync_InsideCircuit_WhenTokenIsEmpty_DoesNotAttachAuthorizationHeader()
    {
        _circuitServicesAccessor.Services = CreateCircuitServices(string.Empty);

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
    }

    [Fact]
    public async Task SendAsync_OutsideCircuit_WhenHttpContextHasToken_AttachesBearerAuthorizationHeader()
    {
        _httpContextAccessor.HttpContext.Returns(CreateHttpContextWithToken("prerender-token"));

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest!.Headers.Authorization.Should().NotBeNull();
        _innerHandler.LastRequest.Headers.Authorization!.Parameter.Should().Be("prerender-token");
    }

    [Fact]
    public async Task SendAsync_OutsideCircuit_WhenNoHttpContext_DoesNotAttachAuthorizationHeader()
    {
        _httpContextAccessor.HttpContext.Returns((HttpContext?)null);

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
    }

    /// <summary>
    /// Regression test for the cross-user token leak: one handler instance (as pooled by
    /// HttpClientFactory) must use whichever circuit is executing at the moment of each
    /// request, never a token cached from an earlier user.
    /// </summary>
    [Fact]
    public async Task SendAsync_SameHandlerInstanceUsedByTwoCircuits_UsesEachCircuitsOwnToken()
    {
        using var client = CreateClient();

        _circuitServicesAccessor.Services = CreateCircuitServices("platform-user-token");
        await client.GetAsync("api/v1/users");
        var firstAuthorization = _innerHandler.LastRequest!.Headers.Authorization;

        _circuitServicesAccessor.Services = CreateCircuitServices("holding-user-token");
        await client.GetAsync("api/v1/users");
        var secondAuthorization = _innerHandler.LastRequest!.Headers.Authorization;

        firstAuthorization!.Parameter.Should().Be("platform-user-token");
        secondAuthorization!.Parameter.Should().Be("holding-user-token");
    }

    /// <summary>Captures the outgoing request and returns a canned 200 OK, without any real network call.</summary>
    private sealed class RecordingHandler : DelegatingHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }
}