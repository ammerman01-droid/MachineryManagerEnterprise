using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Tests;
using MachineryManagerEnterprise.Identity.Infrastructure.Options;
using MachineryManagerEnterprise.Identity.Infrastructure.Persistence;
using MachineryManagerEnterprise.Testing.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests.Persistence;

/// <summary>
/// Integration tests for <see cref="IdentityDataSeeder"/> against a real
/// SQL Server (the shared Testing.Common container, TE-0030 / ADR-0024):
/// Identity + OpenIddict managers are the seams the seeder actually
/// depends on, so mocking them would mostly re-test NSubstitute rather
/// than the seeding logic.
/// </summary>
[Collection(IdentitySqlServerCollection.Name)]
public sealed class IdentityDataSeederTests : IAsyncLifetime
{
    private static readonly OpenIddictClientOptions ValidClientOptions = new()
    {
        Web = new ClientOptions { ClientId = "web-client", ClientSecret = "web-secret" },
        Maui = new ClientOptions { ClientId = "maui-client" },
        Service = new ClientOptions { ClientId = "service-client", ClientSecret = "service-secret" },
    };

    private readonly SqlServerContainerFixture _containerFixture;
    private string _connectionString = string.Empty;

    public IdentityDataSeederTests(SqlServerContainerFixture containerFixture)
    {
        _containerFixture = containerFixture;
    }

    public async Task InitializeAsync()
    {
        _connectionString = TestDatabaseNaming.UniqueConnectionString(
            _containerFixture.ConnectionString, nameof(IdentityDataSeederTests));

        await using var provider = BuildServiceProvider(Environments.Development, ValidClientOptions);
        await using var context = provider.GetRequiredService<IdentityDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var provider = BuildServiceProvider(Environments.Development, ValidClientOptions);
        await using var context = provider.GetRequiredService<IdentityDbContext>();
        await context.Database.EnsureDeletedAsync();
    }

    private ServiceProvider BuildServiceProvider(string environmentName, OpenIddictClientOptions clientOptions)
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddDataProtection(); // required by AddDefaultTokenProviders()'s DataProtectorTokenProvider
        // Fully qualified: this file's enclosing namespace
        // (MachineryManagerEnterprise.Identity.Infrastructure.Tests) is a
        // sibling of the production MachineryManagerEnterprise.Identity.
        // Infrastructure.Options namespace, and C# resolves an unqualified
        // "Options" against enclosing-namespace members before considering
        // `using Microsoft.Extensions.Options;` — so a bare `Options.Create`
        // here binds to that namespace, not the intended static class.
        services.AddSingleton(Microsoft.Extensions.Options.Options.Create(clientOptions));
        services.AddSingleton<IHostEnvironment>(new FakeHostEnvironment { EnvironmentName = environmentName });

        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(
                _connectionString,
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", schema: "identity"));
            options.UseOpenIddict<Guid>();
        });

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddOpenIddict()
            .AddCore(options => options
                .UseEntityFrameworkCore()
                .UseDbContext<IdentityDbContext>()
                .ReplaceDefaultEntities<Guid>());

        return services.BuildServiceProvider();
    }

    private async Task SeedAsync(string environmentName, OpenIddictClientOptions? clientOptions = null)
    {
        await using var provider = BuildServiceProvider(environmentName, clientOptions ?? ValidClientOptions);
        var environment = provider.GetRequiredService<IHostEnvironment>();

        await IdentityDataSeeder.SeedAsync(provider, environment);
    }

    private async Task<ServiceProvider> OpenScopeAsync() =>
        BuildServiceProvider(Environments.Development, ValidClientOptions);

    [Fact]
    public async Task SeedAsync_CreatesAllElevenStandardRoles()
    {
        await SeedAsync(Environments.Development);

        await using var provider = await OpenScopeAsync();
        var roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();

        foreach (var roleName in StandardRoles.All)
        {
            (await roleManager.RoleExistsAsync(roleName)).Should().BeTrue($"role '{roleName}' should have been seeded");
        }
    }

    [Fact]
    public async Task SeedAsync_InDevelopment_CreatesTheTestUserWithSystemAdministratorRole()
    {
        await SeedAsync(Environments.Development);

        await using var provider = await OpenScopeAsync();
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByNameAsync("sysadmin");
        user.Should().NotBeNull();

        var roles = await userManager.GetRolesAsync(user!);
        roles.Should().Contain(StandardRoles.SystemAdministrator);
    }

    [Fact]
    public async Task SeedAsync_OutsideDevelopment_DoesNotCreateTheTestUser()
    {
        await SeedAsync(Environments.Production);

        await using var provider = await OpenScopeAsync();
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByNameAsync("sysadmin");
        user.Should().BeNull();
    }

    [Fact]
    public async Task SeedAsync_OutsideDevelopment_StillSeedsRolesAndClients()
    {
        await SeedAsync(Environments.Production);

        await using var provider = await OpenScopeAsync();
        var roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        var applicationManager = provider.GetRequiredService<IOpenIddictApplicationManager>();

        (await roleManager.RoleExistsAsync(StandardRoles.SystemAdministrator)).Should().BeTrue();
        (await applicationManager.FindByClientIdAsync(ValidClientOptions.Web.ClientId)).Should().NotBeNull();
    }

    [Fact]
    public async Task SeedAsync_SeedsWebMauiAndServiceOpenIddictApplications()
    {
        await SeedAsync(Environments.Development);

        await using var provider = await OpenScopeAsync();
        var applicationManager = provider.GetRequiredService<IOpenIddictApplicationManager>();

        (await applicationManager.FindByClientIdAsync(ValidClientOptions.Web.ClientId)).Should().NotBeNull();
        (await applicationManager.FindByClientIdAsync(ValidClientOptions.Maui.ClientId)).Should().NotBeNull();
        (await applicationManager.FindByClientIdAsync(ValidClientOptions.Service.ClientId)).Should().NotBeNull();
    }

    [Fact]
    public async Task SeedAsync_CalledTwice_IsIdempotent()
    {
        await SeedAsync(Environments.Development);
        await SeedAsync(Environments.Development);

        await using var provider = await OpenScopeAsync();
        var roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

        roleManager.Roles.Count(r => r.Name == StandardRoles.SystemAdministrator).Should().Be(1);
        userManager.Users.Count(u => u.UserName == "sysadmin").Should().Be(1);
    }

    [Fact]
    public async Task SeedAsync_WithoutWebClientId_ThrowsInvalidOperationException()
    {
        var incompleteOptions = new OpenIddictClientOptions
        {
            Web = new ClientOptions { ClientId = string.Empty },
            Maui = ValidClientOptions.Maui,
            Service = ValidClientOptions.Service,
        };

        var act = () => SeedAsync(Environments.Development, incompleteOptions);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Web:ClientId*");
    }

    [Fact]
    public async Task SeedAsync_WithoutWebClientSecret_ThrowsInvalidOperationException()
    {
        var incompleteOptions = new OpenIddictClientOptions
        {
            Web = new ClientOptions { ClientId = "web-client", ClientSecret = null },
            Maui = ValidClientOptions.Maui,
            Service = ValidClientOptions.Service,
        };

        var act = () => SeedAsync(Environments.Development, incompleteOptions);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Web:ClientSecret*");
    }
}
