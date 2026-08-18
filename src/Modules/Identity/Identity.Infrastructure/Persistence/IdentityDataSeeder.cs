using MachineryManager.Identity.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManager.Identity.Infrastructure.Persistence;

/// <summary>
/// Seeds the Identity module's infrastructure data — currently, the
/// OpenIddict Application (client) registrations sourced from
/// <see cref="OpenIddictClientOptions"/> (configuration / User Secrets).
/// </summary>
public static class IdentityDataSeeder
{
    /// <summary>Seeds OpenIddict applications and other Identity infrastructure data.</summary>
    /// <param name="serviceProvider">The application's service provider.</param>
    /// <returns>A task representing the asynchronous seeding operation.</returns>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var applicationManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var clientOptions = serviceProvider
            .GetRequiredService<IOptions<OpenIddictClientOptions>>()
            .Value;

        await SeedWebClientAsync(applicationManager, clientOptions.Web);
        await SeedMauiClientAsync(applicationManager, clientOptions.Maui);
        await SeedServiceClientAsync(applicationManager, clientOptions.Service);
    }

    /// <summary>Seeds the confidential Web application (Authorization Code + PKCE).</summary>
    private static async Task SeedWebClientAsync(
        IOpenIddictApplicationManager applicationManager,
        ClientOptions client)
    {
        if (string.IsNullOrWhiteSpace(client.ClientId))
        {
            throw new InvalidOperationException("OpenIddict:Clients:Web:ClientId is not configured.");
        }

        if (await applicationManager.FindByClientIdAsync(client.ClientId) is not null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(client.ClientSecret))
        {
            // Fail fast rather than silently registering a Confidential
            // client with no real secret — that would be an
            // authentication bypass waiting to happen.
            throw new InvalidOperationException(
                "OpenIddict:Clients:Web:ClientSecret is not configured. Set it via " +
                "'dotnet user-secrets' (Development) or your secret store (Production) " +
                "— it must never be committed to appsettings.json.");
        }

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = client.ClientId,
            ClientSecret = client.ClientSecret,
            ClientType = ClientTypes.Confidential,
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "MachineryManager Web",
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

    /// <summary>Seeds the public MAUI application (Authorization Code + PKCE, no client secret).</summary>
    private static async Task SeedMauiClientAsync(
        IOpenIddictApplicationManager applicationManager,
        ClientOptions client)
    {
        if (string.IsNullOrWhiteSpace(client.ClientId))
        {
            throw new InvalidOperationException("OpenIddict:Clients:Maui:ClientId is not configured.");
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
            DisplayName = "MachineryManager MAUI",
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

    /// <summary>Seeds the confidential Service-to-Service application (Client Credentials).</summary>
    private static async Task SeedServiceClientAsync(
        IOpenIddictApplicationManager applicationManager,
        ClientOptions client)
    {
        if (string.IsNullOrWhiteSpace(client.ClientId))
        {
            throw new InvalidOperationException("OpenIddict:Clients:Service:ClientId is not configured.");
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
            DisplayName = "MachineryManager Service",
        };

        descriptor.Permissions.Add(Permissions.Endpoints.Token);
        descriptor.Permissions.Add(Permissions.GrantTypes.ClientCredentials);

        await applicationManager.CreateAsync(descriptor);
    }
}