using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Persistence;
using MachineryManagerEnterprise.Identity.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Identity.Presentation.Tests.Infrastructure;

/// <summary>
/// Builds an in-memory ASP.NET Core host ("module slice", not the real
/// Host project — see Identity.Presentation.Tests.csproj) that maps
/// only <c>UserEndpoints</c>, backed by a real
/// <c>UserManager&lt;ApplicationUser&gt;</c> over a real SQL Server
/// database (a fresh one per test class — see
/// <see cref="IdentitySqlServerCollection"/> /
/// <see cref="TestDatabaseNaming"/>), with
/// <see cref="RoleAwareTestAuthHandler"/> standing in for OpenIddict
/// token validation.
/// </summary>
public sealed class UserEndpointsTestHost : IAsyncDisposable
{
    private readonly IHost _host;

    private UserEndpointsTestHost(IHost host)
    {
        _host = host;
    }

    /// <summary>An <see cref="HttpClient"/> talking directly to the in-memory server (no real network).</summary>
    public HttpClient CreateClient() => _host.GetTestClient();

    /// <summary>Opens a DI scope, for tests that need to seed data directly (e.g. via UserManager) before making a request.</summary>
    /// <remarks>
    /// Returns <see cref="AsyncServiceScope"/> (not the plain
    /// <see cref="IServiceScope"/> from <c>CreateScope()</c>) specifically
    /// so callers can dispose it with <c>await using</c> — needed because
    /// disposing the scope also disposes the scoped
    /// <see cref="Identity.Infrastructure.Persistence.IdentityDbContext"/>
    /// it created, whose own disposal is asynchronous.
    /// </remarks>
    public AsyncServiceScope CreateScope() => _host.Services.CreateAsyncScope();

    /// <param name="connectionString">
    /// A connection string to an already-created, uniquely-named
    /// database (see <see cref="TestDatabaseNaming"/>). This method
    /// creates that database's schema (EnsureCreated); it does not
    /// create the database server/container itself.
    /// </param>
    public static async Task<UserEndpointsTestHost> CreateAsync(string connectionString)
    {
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();

                webHost.ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddAuthorization();

                    services
                        .AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                        .AddScheme<RoleAwareTestAuthHandlerOptions, RoleAwareTestAuthHandler>(
                            OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
                            configureOptions: null);

                    services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

                    services
                        .AddIdentityCore<ApplicationUser>(options =>
                        {
                            // Endpoint-behavior tests aside from validators
                            // themselves — relax password rules so tests can
                            // use short, simple fixture passwords without
                            // that policy being incidental noise here (the
                            // real policy is exercised by
                            // Identity.Infrastructure.Tests directly).
                            options.Password.RequiredLength = 1;
                            options.Password.RequireDigit = false;
                            options.Password.RequireLowercase = false;
                            options.Password.RequireUppercase = false;
                            options.Password.RequireNonAlphanumeric = false;
                            options.User.RequireUniqueEmail = false;
                        })
                        .AddRoles<ApplicationRole>()
                        .AddEntityFrameworkStores<IdentityDbContext>()
                        .AddDefaultTokenProviders();
                });

                webHost.Configure(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints => ((IEndpointRouteBuilder)endpoints).MapIdentityUserEndpoints());
                });
            });

        var host = await hostBuilder.StartAsync();

        await using (var scope = host.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            await context.Database.EnsureCreatedAsync();

            // UserEndpoints assigns roles by name (CreateUserAsync,
            // AddToRoleAsync) — ASP.NET Core Identity's default UserStore
            // requires the role to already exist, or throws
            // InvalidOperationException. This host doesn't run the real
            // IdentityDataSeeder (that's covered by
            // Identity.Infrastructure.Tests, and pulls in OpenIddict Core
            // registrations this endpoint slice doesn't need) — it just
            // seeds the closed StandardRoles catalog directly, which is
            // all UserEndpoints itself depends on.
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            foreach (var roleName in StandardRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }
        }

        return new UserEndpointsTestHost(host);
    }

    public async ValueTask DisposeAsync()
    {
        await _host.StopAsync();
        _host.Dispose();
    }
}
