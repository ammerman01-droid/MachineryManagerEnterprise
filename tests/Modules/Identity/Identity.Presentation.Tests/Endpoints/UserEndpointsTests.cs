using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Persistence;
using MachineryManagerEnterprise.Identity.Presentation.Contracts;
using MachineryManagerEnterprise.Identity.Presentation.Tests.Infrastructure;
using MachineryManagerEnterprise.Testing.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Presentation.Tests.Endpoints;

/// <summary>
/// Integration tests for <c>UserEndpoints</c> (<c>/api/v1/users/*</c>)
/// via an in-memory TestServer (see <see cref="UserEndpointsTestHost"/>).
/// Covers routing, model binding, the endpoints' own business logic,
/// and the authorization policies actually declared in
/// <c>MapIdentityUserEndpoints</c> (RequireAuthenticatedUser /
/// RequireClaim "System Administrator") — not OpenIddict token
/// validation itself, which is simulated by
/// <see cref="RoleAwareTestAuthHandler"/> (see its own remarks for why).
/// </summary>
[Collection(IdentitySqlServerCollection.Name)]
public sealed class UserEndpointsTests : IAsyncLifetime
{
    // The minimal API endpoints serialize with camelCase property names
    // (ASP.NET Core's default JsonOptions), while the response DTOs used
    // here for deserialization are ordinary PascalCase C# types —
    // case-insensitive matching bridges the two.
    private static readonly JsonSerializerOptions ResponseJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly SqlServerContainerFixture _containerFixture;
    private UserEndpointsTestHost _host = null!;
    private HttpClient _client = null!;

    public UserEndpointsTests(SqlServerContainerFixture containerFixture)
    {
        _containerFixture = containerFixture;
    }

    public async Task InitializeAsync()
    {
        var connectionString = TestDatabaseNaming.UniqueConnectionString(
            _containerFixture.ConnectionString, nameof(UserEndpointsTests));

        _host = await UserEndpointsTestHost.CreateAsync(connectionString);
        _client = _host.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await using (var scope = _host.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            await context.Database.EnsureDeletedAsync();
        }

        _client.Dispose();
        await _host.DisposeAsync();
    }

    private static HttpRequestMessage Request(HttpMethod method, string url, bool authenticated = false, string? role = null)
    {
        var request = new HttpRequestMessage(method, url);

        if (authenticated)
        {
            request.Headers.Add(RoleAwareTestAuthHandler.AuthenticatedHeader, "true");
        }

        if (role is not null)
        {
            request.Headers.Add(RoleAwareTestAuthHandler.RoleHeader, role);
        }

        return request;
    }

    private async Task<Guid> SeedUserAsync(string userName, string password = "Test1", string? role = null)
    {
        await using var scope = _host.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser { UserName = userName };
        var result = await userManager.CreateAsync(user, password);
        result.Succeeded.Should().BeTrue(string.Join("; ", result.Errors.Select(e => e.Description)));

        if (role is not null)
        {
            await userManager.AddToRoleAsync(user, role);
        }

        return user.Id;
    }

    // ---- Authentication ----

    [Theory]
    [InlineData("GET", "/api/v1/users")]
    [InlineData("POST", "/api/v1/users")]
    [InlineData("GET", "/api/v1/users/11111111-1111-1111-1111-111111111111")]
    [InlineData("GET", "/api/v1/users/11111111-1111-1111-1111-111111111111/roles")]
    [InlineData("POST", "/api/v1/users/11111111-1111-1111-1111-111111111111/deactivate")]
    [InlineData("POST", "/api/v1/users/11111111-1111-1111-1111-111111111111/activate")]
    public async Task Endpoint_WithoutAuthentication_Returns401(string method, string url)
    {
        var response = await _client.SendAsync(Request(new HttpMethod(method), url));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("POST", "/api/v1/users")]
    [InlineData("POST", "/api/v1/users/11111111-1111-1111-1111-111111111111/deactivate")]
    [InlineData("POST", "/api/v1/users/11111111-1111-1111-1111-111111111111/activate")]
    public async Task AdminOnlyEndpoint_AuthenticatedWithoutSystemAdministratorRole_Returns403(string method, string url)
    {
        var response = await _client.SendAsync(Request(new HttpMethod(method), url, authenticated: true));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // ---- CreateUser ----

    [Fact]
    public async Task CreateUser_AsSystemAdministrator_Returns201AndPersistsTheUser()
    {
        var request = Request(HttpMethod.Post, "/api/v1/users", authenticated: true, role: StandardRoles.SystemAdministrator);
        request.Content = JsonContent.Create(new CreateUserRequest("new.operator", "Test1", [StandardRoles.Operator]));

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var created = await response.Content.ReadFromJsonAsync<UserDto>(ResponseJsonOptions);
        created.Should().NotBeNull();
        created!.UserName.Should().Be("new.operator");
        created.IsActive.Should().BeTrue();

        await using var scope = _host.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var persisted = await userManager.FindByIdAsync(created.Id.ToString());
        persisted.Should().NotBeNull();
        (await userManager.GetRolesAsync(persisted!)).Should().Contain(StandardRoles.Operator);
    }

    [Fact]
    public async Task CreateUser_IgnoresRolesNotInTheStandardCatalog()
    {
        var request = Request(HttpMethod.Post, "/api/v1/users", authenticated: true, role: StandardRoles.SystemAdministrator);
        request.Content = JsonContent.Create(new CreateUserRequest("odd.role.user", "Test1", ["Not A Real Role"]));

        var response = await _client.SendAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<UserDto>(ResponseJsonOptions);

        await using var scope = _host.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var persisted = await userManager.FindByIdAsync(created!.Id.ToString());
        (await userManager.GetRolesAsync(persisted!)).Should().BeEmpty();
    }

    [Fact]
    public async Task CreateUser_WithPasswordFailingPolicy_Returns400()
    {
        var request = Request(HttpMethod.Post, "/api/v1/users", authenticated: true, role: StandardRoles.SystemAdministrator);
        request.Content = JsonContent.Create(new CreateUserRequest("bad.password.user", string.Empty));

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ---- GetUserById ----

    [Fact]
    public async Task GetUserById_Unknown_Returns404()
    {
        var response = await _client.SendAsync(
            Request(HttpMethod.Get, $"/api/v1/users/{Guid.NewGuid()}", authenticated: true));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserById_Existing_Returns200WithUserDto()
    {
        var userId = await SeedUserAsync("lookup.user");

        var response = await _client.SendAsync(
            Request(HttpMethod.Get, $"/api/v1/users/{userId}", authenticated: true));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<UserDto>(ResponseJsonOptions);
        dto!.UserName.Should().Be("lookup.user");
        dto.IsActive.Should().BeTrue();
    }

    // ---- GetUserRoles ----

    [Fact]
    public async Task GetUserRoles_ReturnsAssignedRoles()
    {
        var userId = await SeedUserAsync("fleet.user", role: StandardRoles.FleetManager);

        var response = await _client.SendAsync(
            Request(HttpMethod.Get, $"/api/v1/users/{userId}/roles", authenticated: true));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var roles = await response.Content.ReadFromJsonAsync<string[]>(ResponseJsonOptions);
        roles.Should().Contain(StandardRoles.FleetManager);
    }

    [Fact]
    public async Task GetUserRoles_Unknown_Returns404()
    {
        var response = await _client.SendAsync(
            Request(HttpMethod.Get, $"/api/v1/users/{Guid.NewGuid()}/roles", authenticated: true));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ---- Deactivate / Activate ----

    [Fact]
    public async Task DeactivateThenActivate_TogglesIsActive()
    {
        var userId = await SeedUserAsync("toggle.user");

        var deactivate = await _client.SendAsync(Request(
            HttpMethod.Post, $"/api/v1/users/{userId}/deactivate", authenticated: true, role: StandardRoles.SystemAdministrator));
        deactivate.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var afterDeactivate = await _client.SendAsync(
            Request(HttpMethod.Get, $"/api/v1/users/{userId}", authenticated: true));
        (await afterDeactivate.Content.ReadFromJsonAsync<UserDto>(ResponseJsonOptions))!.IsActive.Should().BeFalse();

        var activate = await _client.SendAsync(Request(
            HttpMethod.Post, $"/api/v1/users/{userId}/activate", authenticated: true, role: StandardRoles.SystemAdministrator));
        activate.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var afterActivate = await _client.SendAsync(
            Request(HttpMethod.Get, $"/api/v1/users/{userId}", authenticated: true));
        (await afterActivate.Content.ReadFromJsonAsync<UserDto>(ResponseJsonOptions))!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task DeactivateUser_Unknown_Returns404()
    {
        var response = await _client.SendAsync(Request(
            HttpMethod.Post, $"/api/v1/users/{Guid.NewGuid()}/deactivate", authenticated: true, role: StandardRoles.SystemAdministrator));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ---- SearchUsers (pagination) ----

    [Fact]
    public async Task SearchUsers_PaginatesResults()
    {
        for (var i = 0; i < 5; i++)
        {
            await SeedUserAsync($"page.user.{i}");
        }

        var response = await _client.SendAsync(
            Request(HttpMethod.Get, "/api/v1/users?page=1&pageSize=2", authenticated: true));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<SearchUsersResponse>(ResponseJsonOptions);

        payload!.Items.Should().HaveCount(2);
        payload.Page.Should().Be(1);
        payload.PageSize.Should().Be(2);
        payload.TotalItems.Should().Be(5);
        payload.HasNextPage.Should().BeTrue();
        payload.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public async Task SearchUsers_WithSearchTerm_FiltersItemsAndTotalsConsistently()
    {
        // Regression test for a real bug found in production code
        // (chat, 2026-09-21): `items` was sliced from the unfiltered user
        // list before `search` was applied, so the term affected only
        // totalItems/totalPages, never the actual returned rows. Fixed by
        // filtering before slicing/counting — both must now agree.
        await SeedUserAsync("findme.alpha");
        await SeedUserAsync("findme.beta");
        await SeedUserAsync("other.user");

        var response = await _client.SendAsync(
            Request(HttpMethod.Get, "/api/v1/users?search=findme", authenticated: true));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<SearchUsersResponse>(ResponseJsonOptions);

        payload!.Items.Should().HaveCount(2);
        payload.Items.Should().OnlyContain(u => u.UserName.Contains("findme"));
        payload.TotalItems.Should().Be(2, "totalItems must match the actually-returned, filtered items");
    }

    private sealed record SearchUsersResponse(
        UserDto[] Items, int Page, int PageSize, int TotalItems, int TotalPages, bool HasNextPage, bool HasPreviousPage);
}
