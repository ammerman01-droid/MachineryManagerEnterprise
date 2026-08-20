using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.Administration.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.Administration.Infrastructure;

/// <summary>
/// Registers the Administration module's Infrastructure layer services
/// (ADR-0006 EF Core, ADR-0019 Hybrid Persistence Strategy).
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Administration module's <see cref="AdministrationDbContext"/>,
    /// repositories, and unit of work.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAdministrationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AdministrationDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("MachineryManagerDatabase")));

        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IUserProfileAssignmentRepository, UserProfileAssignmentRepository>();
        services.AddScoped<IAdministrationUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<AdministrationDbContext>());

        return services;
    }
}