using MachineryManager.Identity.Domain;
using MachineryManager.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using OpenIddict.Client.AspNetCore;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManager.Identity.Infrastructure;

/// <summary>
/// Registers the Identity platform module's Infrastructure layer
/// services (ADR-0006 EF Core, ADR-0030 Identity &amp; Access Management).
/// </summary>

public static class DependencyInjection
{
    /// <summary>
    /// Registers the <see cref="Persistence.IdentityDbContext"/> and
    /// ASP.NET Core Identity (local user/role store, ADR-0030).
    /// </summary>
    /// <remarks>
    /// The password and lockout policy values set here are NOT sourced
    /// from approved documentation — no numeric password policy exists
    /// anywhere in 02-architecture (ADR-0026) or 03-domain. These are
    /// provisional, OWASP-aligned defaults, flagged explicitly here as
    /// an open item pending approval, not a silently assumed business
    /// rule.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("MachineryManagerDatabase"),
                sqlServerOptions => sqlServerOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    schema: "identity"));
                    
            // Required by OpenIddict.EntityFrameworkCore so its stores
            // (Applications, Authorizations, Scopes, Tokens) are
            // recognized by this DbContext's model.

            options.UseOpenIddict<Guid>();
        });

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                // Password Policy — explicitly specified by the product
                // owner (chat, 2026-08-18), not a library default.
                options.Password.RequiredLength = PasswordPolicy.MinLength;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                // Lockout policy: still provisional (OWASP-aligned
                // default) — not yet explicitly confirmed by the
                // product owner. Flagged as a remaining open item.
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                // No email collection (product owner, chat 2026-08-18):
                // users are created by an Administrator only (05-application
                // Section 5.3, Administration module), without an email
                // address, so email uniqueness does not apply.
                options.User.RequireUniqueEmail = false;

                // English letters, digits, and standard ASCII special
                // characters only (product owner, chat 2026-08-18) — this
                // matches ASP.NET Core Identity's own default character
                // set, restated explicitly so the rule is traceable in
                // code rather than an implicit library default.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddUserValidator<Validation.UsernameLengthValidator>()
            .AddPasswordValidator<Validation.AllowedCharactersPasswordValidator>()
            // Added now: AddIdentityCore alone does NOT register
            // SignInManager (unlike the full AddIdentity()). Required
            // for the interactive Login page (cookie-based sign-in).
            .AddSignInManager();

        services
            .AddOptions<Options.OpenIddictClientOptions>()
            .BindConfiguration(Options.OpenIddictClientOptions.SectionName)
            .ValidateOnStart();

        return services;
    }

    /// <summary>
    /// Registers the OpenIddict Authorization Server + Token Validation
    /// (ADR-0030), configured for two flows per the current requirement:
    /// Authorization Code + PKCE (interactive users — Blazor Server,
    /// MAUI) and Client Credentials (Service-to-Service integrations).
    /// </summary>
    /// <remarks>
    /// Production signing/encryption certificates are NOT configured
    /// here — no certificate source (file, Key Vault, etc.) is
    /// documented anywhere in ADR-0026. Rather than inventing one, this
    /// method fails fast outside Development until that decision is
    /// made and documented.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="environment">The hosting environment, used to select Development-only certificates.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddIdentityOpenIddictServer(
        this IServiceCollection services,
        IHostEnvironment environment)
    {
        // Cookie authentication for the interactive Login page.
        // Fixes an omission from the previous step: without this, there
        // is no authentication scheme available for SignInManager to
        // issue a sign-in cookie against.
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            })
            .AddIdentityCookies();

        services
            .AddOpenIddict()
            .AddCore(options => options
                .UseEntityFrameworkCore()
                .UseDbContext<IdentityDbContext>()
                .ReplaceDefaultEntities<Guid>())
            .AddServer(options =>
            {
                options
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserInfoEndpointUris("/connect/userinfo")
                    .SetEndSessionEndpointUris("/connect/logout");

                options
                    .AllowAuthorizationCodeFlow()
                    .RequireProofKeyForCodeExchange();

                options.AllowClientCredentialsFlow();
                options.AllowRefreshTokenFlow();

                options.RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Roles);

                if (environment.IsDevelopment())
                {
                    options
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }
                else
                {
                    throw new InvalidOperationException(
                        "Production OpenIddict signing/encryption certificates are not configured. " +
                        "This requires an explicit decision (documented in ADR-0026) on the certificate source " +
                        "(e.g., Azure Key Vault, mounted X.509 file) before this environment can start.");
                }

                options
                    .UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddAuthorization();

        return services;
    }

        /// <summary>
    /// Registers this application as an OpenIddict Client of its own
    /// Authorization Server (self-referencing monolith pattern), so
    /// Blazor Server can complete the Authorization Code + PKCE flow
    /// and obtain an access token to call protected APIs later.
    /// </summary>
    /// <remarks>
    /// The user's local session (ApplicationScheme cookie) is already
    /// established at password sign-in (/identity/login) — this client
    /// does not "log the user in" a second time; it obtains and stores
    /// the access/refresh tokens on that same session.
    /// </remarks>
    public static IServiceCollection AddIdentityOpenIddictClient(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var webClient = configuration
            .GetSection($"{Options.OpenIddictClientOptions.SectionName}:Web")
            .Get<Options.ClientOptions>()
            ?? throw new InvalidOperationException($"{Options.OpenIddictClientOptions.SectionName}:Web is not configured.");

        if (string.IsNullOrWhiteSpace(webClient.ClientSecret))
        {
            throw new InvalidOperationException(
                "OpenIddict:Clients:Web:ClientSecret is not configured (required for the OpenIddict Client too).");
        }

        var issuer = configuration["OpenIddict:Issuer"]
            ?? throw new InvalidOperationException("OpenIddict:Issuer is not configured.");

        services
            .AddOpenIddict()
            .AddClient(options =>
            {
                options.AllowAuthorizationCodeFlow();

                if (environment.IsDevelopment())
                {
                    options
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }
                else
                {
                    throw new InvalidOperationException(
                        "Production OpenIddict Client certificates are not configured (same open item as the Server — ADR-0026).");
                }

                options
                    .UseAspNetCore()
                    .EnableStatusCodePagesIntegration()
                    .EnableRedirectionEndpointPassthrough();

                options.UseSystemNetHttp();

                options.AddRegistration(new OpenIddictClientRegistration
                {
                    Issuer = new Uri(issuer, UriKind.Absolute),
                    ClientId = webClient.ClientId,
                    ClientSecret = webClient.ClientSecret,
                    RedirectUri = new Uri("/signin-oidc", UriKind.Relative),
                    // Scopes.OfflineAccess added (chat, 2026-08-22):
                    // without it, OpenIddict never issues a refresh_token.
                    Scopes = { Scopes.OpenId, Scopes.Profile, Scopes.Email, Scopes.Roles, Scopes.OfflineAccess },
                });
            });

        return services;
    }

        /// <summary>
    /// Registers a named HttpClient ("InternalApi") that automatically
    /// attaches the current user's Bearer access token to every
    /// request, for Blazor Server pages calling this monolith's own
    /// protected API endpoints (chat, 2026-08-22).
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the base address.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddIdentityInternalApiClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<Http.BearerTokenHandler>();

        var baseAddress = configuration["OpenIddict:Issuer"]
            ?? throw new InvalidOperationException("OpenIddict:Issuer is not configured.");

        services
            .AddHttpClient("InternalApi", client => client.BaseAddress = new Uri(baseAddress, UriKind.Absolute))
            .AddHttpMessageHandler<Http.BearerTokenHandler>();

        return services;
    }
}