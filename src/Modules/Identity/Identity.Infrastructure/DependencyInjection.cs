using MachineryManager.Identity.Domain;
using MachineryManager.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddDbContext<IdentityDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("MachineryManagerDatabase")));

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
}