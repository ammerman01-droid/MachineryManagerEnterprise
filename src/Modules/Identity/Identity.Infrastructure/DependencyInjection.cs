using MachineryManager.Identity.Domain;
using MachineryManager.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

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
                // Provisional OWASP-aligned defaults — see remarks above.
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

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
                    // Open item: production certificate source is not
                    // documented in ADR-0026. Failing fast rather than
                    // silently running with an insecure/invented default.
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
                // Local server validation: this same monolith issues and
                // validates its own tokens (single OpenIddict server).
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddAuthorization();

        return services;
    }
}