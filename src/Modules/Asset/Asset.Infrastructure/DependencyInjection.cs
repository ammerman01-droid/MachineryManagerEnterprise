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
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAssetInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AssetDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("MachineryManagerDatabase")));

        services.AddScoped<IAssetModelRepository, AssetModelRepository>();
        services.AddScoped<IEngineModelRepository, EngineModelRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<AssetDbContext>());

        return services;
    }
}