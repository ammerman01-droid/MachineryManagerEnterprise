using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Infrastructure.Persistence;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.Asset.Infrastructure;

/// <summary>Registers the Asset module's Infrastructure-layer services.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Asset module's DbContext, repositories, and Unit
    /// of Work with the dependency injection container.
    /// </summary>
    /// <remarks>
    /// Uses the shared <c>MachineryManagerDatabase</c> connection string
    /// key, matching Organization and Administration's infrastructure
    /// registration convention.
    /// Registers <see cref="IAssetUnitOfWork"/>, not the shared
    /// <see cref="IUnitOfWork"/> directly (chat, 2026-08-27 fix — the
    /// shared interface caused a cross-module DI collision: whichever
    /// module's <c>AddScoped&lt;IUnitOfWork&gt;</c> ran last in
    /// Program.cs silently won for the entire application, so
    /// Organization's SaveChangesAsync calls were resolving to
    /// AssetDbContext instead and discarding their changes).
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAssetInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AssetDbContext>(options => options
            .UseSqlServer(
                configuration.GetConnectionString("MachineryManagerDatabase"),
                sqlServerOptions => sqlServerOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    schema: "asset")));

        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssetModelRepository, AssetModelRepository>();
        services.AddScoped<IEngineModelRepository, EngineModelRepository>();
        services.AddScoped<IAssetUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<AssetDbContext>());
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IUnitOfMeasurementRepository, UnitOfMeasurementRepository>();

        return services;
    }
}