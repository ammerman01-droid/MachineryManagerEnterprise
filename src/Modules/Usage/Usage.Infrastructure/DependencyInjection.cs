using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Usage.Infrastructure;

/// <summary>Registers the Usage module's Infrastructure-layer services.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Usage module's DbContext, repositories, read
    /// service, provisioning service, and Unit of Work with the
    /// dependency injection container.
    /// </summary>
    /// <remarks>
    /// Uses the shared <c>MachineryManagerDatabase</c> connection string
    /// key and the "usage" schema, matching Asset/Organization/
    /// Administration's infrastructure registration convention.
    /// Registers <see cref="IUsageUnitOfWork"/>, not the shared
    /// <see cref="MachineryManagerEnterprise.SharedKernel.Abstractions.IUnitOfWork"/>
    /// directly — registering the shared interface caused a
    /// cross-module DI collision when Asset did this (chat,
    /// 2026-08-27): whichever module's registration ran last in
    /// Program.cs silently won for the entire application.
    /// <see cref="IUsageProvisioningService"/> is also registered against
    /// <see cref="MachineryManagerEnterprise.SharedKernel.Abstractions.IUsageProvisioningService"/>
    /// (chat, 2026-09-29), so the Asset module can consume it without
    /// depending on this module directly.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddUsageInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<UsageDbContext>((serviceProvider, options) => options
            .UseSqlServer(
                configuration.GetConnectionString("MachineryManagerDatabase"),
                sqlServerOptions => sqlServerOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    schema: "usage"))
            .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()));

        services.AddScoped<IMeterDeviceRepository, MeterDeviceRepository>();
        services.AddScoped<IUsageLedgerRepository, UsageLedgerRepository>();
        services.AddScoped<IUsageReadService, UsageReadService>();
        services.AddScoped<MachineryManagerEnterprise.SharedKernel.Abstractions.IUsageProvisioningService, UsageProvisioningService>();
        services.AddScoped<IUsageUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<UsageDbContext>());

        return services;
    }
}
