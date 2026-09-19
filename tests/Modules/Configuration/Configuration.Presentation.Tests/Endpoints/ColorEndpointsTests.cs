using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Testing.Common;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Configuration.Presentation.Tests.Endpoints;

/// <summary>
/// Groups every Presentation-level Configuration test onto one shared
/// <see cref="SqlServerContainerFixture"/>, same as
/// Configuration.Infrastructure.Tests — each is its own test assembly,
/// so each gets its own container.
/// </summary>
[CollectionDefinition(nameof(ConfigurationApiCollection))]
public sealed class ConfigurationApiCollection : ICollectionFixture<SqlServerContainerFixture>;

/// <summary>
/// End-to-end tests for <c>POST/GET /api/v1/colors</c>: real HTTP request
/// through routing, authentication/authorization, the MediatR pipeline
/// (including FluentValidation), the domain, and a real SQL Server.
/// </summary>
[Collection(nameof(ConfigurationApiCollection))]
public sealed class ColorEndpointsTests : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly ConfigurationApiFactory _api;

    public ColorEndpointsTests(SqlServerContainerFixture dbFixture)
    {
        _api = new ConfigurationApiFactory(dbFixture);
    }

    public Task InitializeAsync() => _api.InitializeAsync();

    public Task DisposeAsync() => _api.DisposeAsync();

    private void ArrangeAuthorizedUser(Guid userId, bool isAuthorized = true)
    {
        _api.CurrentUser.SetAuthenticated(userId);
        _api.PermissionEvaluator
            .HasPermissionAsync(userId, Arg.Any<string>(), Arg.Any<ResourceScope>(), Arg.Any<CancellationToken>())
            .Returns(isAuthorized);
    }

    private sealed record CreatedColorResponse(Guid Id);

    private sealed record ProblemResponse(string ErrorCode, string Title, string Message);

    private sealed record ColorResponse(Guid Id, string Name);

    [Fact]
    public async Task PostColors_WithoutAuthentication_Returns401()
    {
        _api.CurrentUser.SetAnonymous();

        var response = await _api.Client.PostAsJsonAsync(
            "/api/v1/colors",
            new { holdingId = Guid.NewGuid(), name = "Red" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostColors_WhenUserLacksPermission_Returns403WithColorNotAuthorized()
    {
        var holdingId = Guid.NewGuid();
        ArrangeAuthorizedUser(Guid.NewGuid(), isAuthorized: false);
        _api.HoldingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(true);

        var response = await _api.Client.PostAsJsonAsync(
            "/api/v1/colors",
            new { holdingId, name = "Red" });

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var problem = await response.Content.ReadFromJsonAsync<ProblemResponse>(JsonOptions);
        problem!.ErrorCode.Should().Be("Color.NotAuthorized");
    }

    [Fact]
    public async Task PostColors_WhenHoldingDoesNotExist_Returns404()
    {
        var holdingId = Guid.NewGuid();
        ArrangeAuthorizedUser(Guid.NewGuid());
        _api.HoldingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(false);

        var response = await _api.Client.PostAsJsonAsync(
            "/api/v1/colors",
            new { holdingId, name = "Red" });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemResponse>(JsonOptions);
        problem!.ErrorCode.Should().Be("Holding.NotFound");
    }

    [Fact]
    public async Task PostColors_WithAnEmptyName_Returns400FromValidation()
    {
        var holdingId = Guid.NewGuid();
        ArrangeAuthorizedUser(Guid.NewGuid());
        _api.HoldingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(true);

        var response = await _api.Client.PostAsJsonAsync(
            "/api/v1/colors",
            new { holdingId, name = "" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostColors_WithAValidAuthorizedRequest_Returns201AndTheColorIsThenListed()
    {
        var holdingId = Guid.NewGuid();
        ArrangeAuthorizedUser(Guid.NewGuid());
        _api.HoldingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(true);

        var postResponse = await _api.Client.PostAsJsonAsync(
            "/api/v1/colors",
            new { holdingId, name = "Teal" });

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await postResponse.Content.ReadFromJsonAsync<CreatedColorResponse>(JsonOptions);
        created!.Id.Should().NotBeEmpty();

        var getResponse = await _api.Client.GetAsync($"/api/v1/colors?holdingId={holdingId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var colors = await getResponse.Content.ReadFromJsonAsync<List<ColorResponse>>(JsonOptions);
        colors.Should().ContainSingle(c => c.Id == created.Id && c.Name == "Teal");
    }

    [Fact]
    public async Task GetColors_WithoutAuthentication_Returns401()
    {
        _api.CurrentUser.SetAnonymous();

        var response = await _api.Client.GetAsync($"/api/v1/colors?holdingId={Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
