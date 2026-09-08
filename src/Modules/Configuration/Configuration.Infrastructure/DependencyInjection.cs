using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Configuration.Infrastructure;

/// <summary>Registers the Configuration module's Infrastructure-layer services.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Configuration module's DbContext, repositories,
    /// cross-module lookup service, and Unit of Work with the
    /// dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddConfigurationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ConfigurationDbContext>((serviceProvider, options) => options
    .UseSqlServer(configuration.GetConnectionString("MachineryManagerDatabase"))
    .AddInterceptors(serviceProvider.GetRequiredService<MachineryManagerEnterprise.SharedKernel.Infrastructure.AuditSaveChangesInterceptor>()));

        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IUnitOfMeasurementRepository, UnitOfMeasurementRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IConfigurationLookupService, ConfigurationLookupService>();
        services.AddScoped<IConfigurationUnitOfWork>(sp => sp.GetRequiredService<ConfigurationDbContext>());
        services.AddScoped<IUnitOfMeasurementLookupService, UnitOfMeasurementLookupService>();
        services.AddScoped<IFuelTypeRepository, FuelTypeRepository>();

        return services;
    }
}