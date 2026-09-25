using FluentValidation;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Maintenance.Application;

/// <summary>
/// Provides extension methods for registering Maintenance Application
/// layer services into the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR handlers, FluentValidation validators, pipeline
    /// behaviors (Validation per ADR-0036), the domain event dispatcher,
    /// and the Mapster-backed mapper.
    /// </summary>
    /// <remarks>
    /// Mapster mappings are scanned into the shared
    /// <see cref="TypeAdapterConfig.GlobalSettings"/> instance rather
    /// than a module-local instance, mirroring every other module
    /// (chat, 2026-08-xx — avoids the single-DI-singleton collision
    /// documented on Asset.Application's copy of this method).
    /// </remarks>
    public static IServiceCollection AddMaintenanceApplication(
        this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(Behaviors.ValidationBehavior<,>));

        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        TypeAdapterConfig.GlobalSettings.Scan(assembly);
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
