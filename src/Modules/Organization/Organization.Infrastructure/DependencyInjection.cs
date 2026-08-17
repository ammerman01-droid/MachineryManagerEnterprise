using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Infrastructure.Persistence;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.Organization.Infrastructure;

/// <summary>
/// Registers the Organization module's Infrastructure layer services
/// (ADR-0006 EF Core, ADR-0019 Hybrid Persistence Strategy).
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Organization module's <see cref="OrganizationDbContext"/>,
    /// repository, and unit of work.
    /// </summary>
    /// <remarks>
    /// The connection string is resolved from the
    /// <c>MachineryManagerDatabase</c> key. No connection string name
    /// was documented in the approved references, so this key is an
    /// explicit assumption — flagged in the completion report — not a
    /// silently invented convention.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddOrganizationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<OrganizationDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("MachineryManagerDatabase")));

        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<OrganizationDbContext>());

        return services;
    }
}
