using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Persistence;

/// <summary>
/// Seeds the Identity module's infrastructure data: OpenIddict
/// Application (client) registrations, Standard Roles, and — in
/// Development only — a test user for end-to-end validation.
/// </summary>
public static class IdentityDataSeeder
{
    /// <summary>
    /// Development-only test credentials — never seeded outside Development.
    /// </summary>
    private const string TestUserName = "sysadmin";
    private const string TestUserPassword = "P@ssw0rd123";

    /// <summary>
    /// Seeds OpenIddict applications, Standard Roles, and (Development only) a test user.
    /// </summary>
    /// <param name="serviceProvider">The application's service provider.</param>
    /// <param name="environment">The hosting environment.</param>
    /// <returns>A task representing the asynchronous seeding operation.</returns>
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(environment);

        var applicationManager =
            serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager =
            serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = serviceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("IdentityDataSeeder");

        var clientOptions = serviceProvider
            .GetRequiredService<IOptions<OpenIddictClientOptions>>()
            .Value;

        await SeedWebClientAsync(applicationManager, clientOptions.Web);
        await SeedMauiClientAsync(applicationManager, clientOptions.Maui);
        await SeedServiceClientAsync(applicationManager, clientOptions.Service);

        await SeedRolesAsync(roleManager);

        if (environment.IsDevelopment())
        {
            await SeedDevelopmentTestUserAsync(userManager, logger);
        }
    }

    /// <summary>
    /// Seeds the closed catalog of Standard Roles if not already present.
    /// </summary>
    private static async Task SeedRolesAsync(
        RoleManager<ApplicationRole> roleManager)
    {
        foreach (var roleName in StandardRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }
    }

    /// <summary>
    /// Seeds a single Development-only test user, assigned the
    /// System Administrator role, for end-to-end login validation.
    /// </summary>
    /// <remarks>
    /// This user and password are test credentials and must never be
    /// created outside Development.
    /// </remarks>
    private static async Task SeedDevelopmentTestUserAsync(
        UserManager<ApplicationUser> userManager,
        ILogger logger)
    {
        if (await userManager.FindByNameAsync(TestUserName) is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = TestUserName,
            LockoutEnabled = true
        };

        var result = await userManager.CreateAsync(user, TestUserPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(
                $"Failed to seed the Development test user: {errors}");
        }

        await userManager.AddToRoleAsync(user, StandardRoles.SystemAdministrator);

        logger.LogWarning(
            "Development test user seeded — username: {UserName}, password: {Password}. " +
            "This user only exists because the environment is Development.",
            TestUserName,
            TestUserPassword);
    }

    /// <summary>
    /// Seeds the confidential Web client using Authorization Code + PKCE.
    /// Existing registrations are updated to include the required permissions.
    /// </summary>
    private static async Task SeedWebClientAsync(
        IOpenIddictApplicationManager applicationManager,
        ClientOptions client)
    {
        if (string.IsNullOrWhiteSpace(client.ClientId))
        {
            throw new InvalidOperationException(
                "OpenIddict:Clients:Web:ClientId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(client.ClientSecret))
        {
            throw new InvalidOperationException(
                "OpenIddict:Clients:Web:ClientSecret is not configured. Set it via " +
                "'dotnet user-secrets' (Development) or your secret store (Production) " +
                "— it must never be committed to appsettings.json.");
        }

        var existingApplication =
            await applicationManager.FindByClientIdAsync(client.ClientId);

        var webDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = client.ClientId,
            ClientSecret = client.ClientSecret,
            ClientType = ClientTypes.Confidential,
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "MachineryManagerEnterprise Web"
        };

        foreach (var redirectUri in client.RedirectUris)
        {
            webDescriptor.RedirectUris.Add(new Uri(redirectUri));
        }

        foreach (var postLogoutRedirectUri in client.PostLogoutRedirectUris)
        {
            webDescriptor.PostLogoutRedirectUris.Add(new Uri(postLogoutRedirectUri));
        }

        webDescriptor.Permissions.Add(Permissions.Endpoints.Authorization);
        webDescriptor.Permissions.Add(Permissions.Endpoints.Token);
        webDescriptor.Permissions.Add(Permissions.Endpoints.EndSession);
        webDescriptor.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
        webDescriptor.Permissions.Add(Permissions.GrantTypes.RefreshToken);
        webDescriptor.Permissions.Add(Permissions.ResponseTypes.Code);
        webDescriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
        webDescriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
        webDescriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Email);
        webDescriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
        webDescriptor.Requirements.Add(Requirements.Features.ProofKeyForCodeExchange);

        if (existingApplication is null)
        {
            await applicationManager.CreateAsync(webDescriptor);
            return;
        }

        // Preserve existing application settings while synchronizing required permissions.
        var existingDescriptor = new OpenIddictApplicationDescriptor();
        await applicationManager.PopulateAsync(existingDescriptor, existingApplication);

        existingDescriptor.ClientSecret = client.ClientSecret;
        existingDescriptor.ClientType = ClientTypes.Confidential;
        existingDescriptor.ConsentType = ConsentTypes.Explicit;
        existingDescriptor.DisplayName = webDescriptor.DisplayName;

        foreach (var redirectUri in webDescriptor.RedirectUris)
        {
            if (!existingDescriptor.RedirectUris.Contains(redirectUri))
            {
                existingDescriptor.RedirectUris.Add(redirectUri);
            }
        }

        foreach (var postLogoutRedirectUri in webDescriptor.PostLogoutRedirectUris)
        {
            if (!existingDescriptor.PostLogoutRedirectUris.Contains(postLogoutRedirectUri))
            {
                existingDescriptor.PostLogoutRedirectUris.Add(postLogoutRedirectUri);
            }
        }

        foreach (var permission in webDescriptor.Permissions)
        {
            if (!existingDescriptor.Permissions.Contains(permission))
            {
                existingDescriptor.Permissions.Add(permission);
            }
        }

        foreach (var requirement in webDescriptor.Requirements)
        {
            if (!existingDescriptor.Requirements.Contains(requirement))
            {
                existingDescriptor.Requirements.Add(requirement);
            }
        }

        await applicationManager.UpdateAsync(existingApplication, existingDescriptor);
    }

    /// <summary>
    /// Seeds the public MAUI application (Authorization Code + PKCE, no client secret).
    /// </summary>
    private static async Task SeedMauiClientAsync(
        IOpenIddictApplicationManager applicationManager,
        ClientOptions client)
    {
        if (string.IsNullOrWhiteSpace(client.ClientId))
        {
            throw new InvalidOperationException(
                "OpenIddict:Clients:Maui:ClientId is not configured.");
        }

        if (await applicationManager.FindByClientIdAsync(client.ClientId) is not null)
        {
            return;
        }

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = client.ClientId,
            ClientType = ClientTypes.Public,
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "MachineryManagerEnterprise MAUI"
        };

        foreach (var redirectUri in client.RedirectUris)
        {
            descriptor.RedirectUris.Add(new Uri(redirectUri));
        }

        foreach (var postLogoutRedirectUri in client.PostLogoutRedirectUris)
        {
            descriptor.PostLogoutRedirectUris.Add(new Uri(postLogoutRedirectUri));
        }

        descriptor.Permissions.Add(Permissions.Endpoints.Authorization);
        descriptor.Permissions.Add(Permissions.Endpoints.Token);
        descriptor.Permissions.Add(Permissions.Endpoints.EndSession);
        descriptor.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
        descriptor.Permissions.Add(Permissions.GrantTypes.RefreshToken);
        descriptor.Permissions.Add(Permissions.ResponseTypes.Code);
        descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
        descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
        descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Email);
        descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
        descriptor.Requirements.Add(Requirements.Features.ProofKeyForCodeExchange);

        await applicationManager.CreateAsync(descriptor);
    }

    /// <summary>
    /// Seeds the confidential Service-to-Service application (Client Credentials).
    /// </summary>
    private static async Task SeedServiceClientAsync(
        IOpenIddictApplicationManager applicationManager,
        ClientOptions client)
    {
        if (string.IsNullOrWhiteSpace(client.ClientId))
        {
            throw new InvalidOperationException(
                "OpenIddict:Clients:Service:ClientId is not configured.");
        }

        if (await applicationManager.FindByClientIdAsync(client.ClientId) is not null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(client.ClientSecret))
        {
            throw new InvalidOperationException(
                "OpenIddict:Clients:Service:ClientSecret is not configured. Set it via " +
                "'dotnet user-secrets' (Development) or your secret store (Production) " +
                "— it must never be committed to appsettings.json.");
        }

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = client.ClientId,
            ClientSecret = client.ClientSecret,
            ClientType = ClientTypes.Confidential,
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "MachineryManagerEnterprise Service"
        };

        descriptor.Permissions.Add(Permissions.Endpoints.Token);
        descriptor.Permissions.Add(Permissions.GrantTypes.ClientCredentials);

        await applicationManager.CreateAsync(descriptor);
    }
}