using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Http;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests.Http;

/// <summary>
/// Verifies <see cref="BearerTokenHandler"/> attaches (or omits) the
/// Bearer Authorization header based on
/// <see cref="ICurrentAccessTokenAccessor"/> (chat, 2026-08-22).
/// </summary>
public sealed class BearerTokenHandlerTests
{
    private readonly ICurrentAccessTokenAccessor _tokenAccessor = Substitute.For<ICurrentAccessTokenAccessor>();
    private readonly RecordingHandler _innerHandler = new();

    private HttpClient CreateClient()
    {
        var handler = new BearerTokenHandler(_tokenAccessor) { InnerHandler = _innerHandler };
        return new HttpClient(handler) { BaseAddress = new Uri("https://internal-api.local/") };
    }

    [Fact]
    public async Task SendAsync_WhenTokenAvailable_AttachesBearerAuthorizationHeader()
    {
        _tokenAccessor.GetAccessTokenAsync().Returns("test-access-token");

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest.Should().NotBeNull();
        _innerHandler.LastRequest!.Headers.Authorization.Should().NotBeNull();
        _innerHandler.LastRequest.Headers.Authorization!.Scheme.Should().Be("Bearer");
        _innerHandler.LastRequest.Headers.Authorization.Parameter.Should().Be("test-access-token");
    }

    [Fact]
    public async Task SendAsync_WhenTokenIsNull_DoesNotAttachAuthorizationHeader()
    {
        _tokenAccessor.GetAccessTokenAsync().Returns((string?)null);

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
    }

    [Fact]
    public async Task SendAsync_WhenTokenIsEmpty_DoesNotAttachAuthorizationHeader()
    {
        _tokenAccessor.GetAccessTokenAsync().Returns(string.Empty);

        using var client = CreateClient();
        await client.GetAsync("api/v1/users");

        _innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
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
