using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Consumption.Infrastructure;

/// <summary>Registers the Consumption module's Infrastructure-layer services.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Consumption module's DbContext, repository, and
    /// Unit of Work with the dependency injection container.
    /// </summary>
    /// <remarks>
    /// Mirrors Asset.Infrastructure's registration exactly — same shared
    /// <c>MachineryManagerDatabase</c> connection string key, own EF
    /// migrations history table under the module's own schema, and a
    /// module-specific <see cref="IConsumptionUnitOfWork"/> registration
    /// rather than the shared <c>IUnitOfWork</c> directly (chat,
    /// 2026-08-27 fix — avoids the cross-module DI collision documented
    /// on AssetDbContext).
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddConsumptionInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ConsumptionDbContext>((serviceProvider, options) => options
            .UseSqlServer(
                configuration.GetConnectionString("MachineryManagerDatabase"),
                sqlServerOptions => sqlServerOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    schema: "consumption"))
            .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()));

        services.AddScoped<IFuelConsumptionRepository, FuelConsumptionRepository>();
        services.AddScoped<IConsumptionUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<ConsumptionDbContext>());

        return services;
    }
}
