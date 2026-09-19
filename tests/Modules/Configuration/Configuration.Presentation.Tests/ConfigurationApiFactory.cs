using MachineryManagerEnterprise.Configuration.Application;
using MachineryManagerEnterprise.Configuration.Infrastructure;
using MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;
using MachineryManagerEnterprise.Configuration.Presentation.Endpoints;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;
using NSubstitute;
using OpenIddict.Validation.AspNetCore;
using Xunit;

namespace MachineryManagerEnterprise.Testing.Common;

/// <summary>
/// Boots a minimal ASP.NET Core test host wired to the REAL Configuration
/// module — Presentation endpoints, Application/MediatR pipeline
/// (including validation), and Infrastructure/EF Core against a real
/// Testcontainers SQL Server — without booting the rest of the
/// application (Identity, Organization, Administration, ...).
/// </summary>
/// <remarks>
/// This is a deliberate "module slice" host rather than
/// <c>WebApplicationFactory&lt;Program&gt;</c> against the real Host
/// project: booting the full <c>MachineryManagerEnterprise.Web</c> would
/// also run <c>IdentityDataSeeder.SeedAsync</c> and require every other
/// module's DbContext/schema to exist, none of which this test suite is
/// about. The two genuinely cross-module dependencies the Color feature
/// has — <see cref="IHoldingLookupService"/> and
/// <see cref="IPermissionEvaluator"/> — are substituted here so a test
/// can control them directly instead of needing real Holding/Permission
/// data seeded through other modules.
/// </remarks>
public sealed class ConfigurationApiFactory : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _dbFixture;
    private IHost _host = null!;

    /// <summary>An <see cref="HttpClient"/> wired directly to the in-memory test server.</summary>
    public HttpClient Client { get; private set; } = null!;

    /// <summary>
    /// Controls who the next request is attributed to. Call
    /// <see cref="FakeCurrentUserService.SetAuthenticated"/> or
    /// <see cref="FakeCurrentUserService.SetAnonymous"/> before sending a request.
    /// </summary>
    public FakeCurrentUserService CurrentUser { get; } = new();

    /// <summary>Substitute for the cross-module Holding-existence check — arrange per test.</summary>
    public IHoldingLookupService HoldingLookupService { get; } = Substitute.For<IHoldingLookupService>();

    /// <summary>Substitute for the cross-module permission check — arrange per test.</summary>
    public IPermissionEvaluator PermissionEvaluator { get; } = Substitute.For<IPermissionEvaluator>();

    /// <summary>Initializes a new instance of the <see cref="ConfigurationApiFactory"/> class.</summary>
    /// <param name="dbFixture">The shared SQL Server container to point the module's DbContext at.</param>
    public ConfigurationApiFactory(SqlServerContainerFixture dbFixture)
    {
        _dbFixture = dbFixture;
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MachineryManagerDatabase"] = _dbFixture.ConnectionString,
            })
            .Build();

        _host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();

                webBuilder.ConfigureServices(services =>
                {
                    services.AddRouting();

                    // Real Application + Infrastructure wiring for the
                    // Configuration module — same extension methods the
                    // real Host calls in Program.cs.
                    services.AddConfigurationApplication();
                    services.AddConfigurationInfrastructure(configuration);

                    // AddConfigurationInfrastructure requires this interceptor
                    // to already be registered (it resolves it via
                    // GetRequiredService when building ConfigurationDbContext).
                    services.AddScoped<AuditSaveChangesInterceptor>();
                    services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

                    // Shared identity used by both application code and
                    // the fake authentication handler below.
                    services.AddSingleton(CurrentUser);
                    services.AddSingleton<ICurrentUserService>(sp => sp.GetRequiredService<FakeCurrentUserService>());

                    // Cross-module dependencies the Color feature needs
                    // but that belong to other modules not booted here.
                    services.AddSingleton(HoldingLookupService);
                    services.AddSingleton(PermissionEvaluator);

                    // Stands in for real OpenIddict token validation,
                    // registered under the exact scheme name
                    // ColorEndpoints.cs requires.
                    services
                        .AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
                            _ => { });
                    services.AddAuthorization();
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints => endpoints.MapColorEndpoints());
                });
            })
            .StartAsync();

        Client = _host.GetTestClient();

        // The audit.AuditEntry table is owned by the Administration
        // module's migration in production; this slice host never boots
        // that module, so AuditOnlyDbContext creates just that one table
        // here instead — see its own doc comment for why the order below
        // (audit table via EnsureCreatedAsync, THEN this module's tables
        // via CreateTablesAsync, never EnsureCreatedAsync twice) matters.
        var auditOptions = new DbContextOptionsBuilder<AuditOnlyDbContext>()
            .UseSqlServer(_dbFixture.ConnectionString)
            .Options;
        await using (var auditContext = new AuditOnlyDbContext(auditOptions))
        {
            await auditContext.Database.EnsureCreatedAsync();
        }

        using var scope = _host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        // xUnit creates a NEW instance of the test class — and therefore
        // a new ConfigurationApiFactory, calling InitializeAsync again —
        // for EVERY [Fact], but they all share the same collection-scoped
        // database. Unlike EnsureCreatedAsync (idempotent: it checks
        // whether the database exists first), CreateTablesAsync always
        // tries to create its tables, so every test after the first hits
        // "There is already an object named 'Color'...". SQL error 2714
        // is exactly that "object already exists" error — safe to ignore
        // here, since it only means a previous test in this collection
        // already created the schema.
        //
        // No EF Core migrations exist for this module yet (see the same
        // note in Configuration.Infrastructure.Tests) — swap for
        // `MigrateAsync()` on both contexts above once they do.
        var databaseCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        try
        {
            await databaseCreator.CreateTablesAsync();
        }
        catch (SqlException ex) when (ex.Number == 2714)
        {
        }
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        Client.Dispose();
        await _host.StopAsync();
        _host.Dispose();
    }
}