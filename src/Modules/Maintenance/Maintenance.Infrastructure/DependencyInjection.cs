using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Maintenance.Infrastructure;

/// <summary>Registers the Maintenance module's Infrastructure-layer services.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Maintenance module's DbContext, repository,
    /// Work Order number generator, and Unit of Work with the
    /// dependency injection container.
    /// </summary>
    /// <remarks>
    /// Uses the shared <c>MachineryManagerDatabase</c> connection string
    /// key. Registers <see cref="IMaintenanceUnitOfWork"/>, not the
    /// shared <see cref="IUnitOfWork"/> directly, mirroring Asset and
    /// Personnel's fix for the cross-module DI collision described on
    /// their own copies of this method.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddMaintenanceInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MaintenanceDbContext>((serviceProvider, options) => options
            .UseSqlServer(
                configuration.GetConnectionString("MachineryManagerDatabase"),
                sqlServerOptions => sqlServerOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    schema: "maintenance"))
            .AddInterceptors(serviceProvider.GetRequiredService<MachineryManagerEnterprise.SharedKernel.Infrastructure.AuditSaveChangesInterceptor>()));

        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IWorkOrderNumberGenerator, WorkOrderNumberGenerator>();
        services.AddScoped<IMaintenanceUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<MaintenanceDbContext>());

        return services;
    }
}
