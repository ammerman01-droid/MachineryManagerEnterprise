using FluentValidation;
using MachineryManagerEnterprise.Consumption.Application.Behaviors;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Consumption.Application;

/// <summary>
/// Provides extension methods for registering Consumption Application
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
    /// <see cref="TypeAdapterConfig.GlobalSettings"/> instance rather than a
    /// module-local <c>new TypeAdapterConfig()</c>, mirroring Asset.Application
    /// — registering a module-local instance as a DI singleton for the same
    /// service type causes every earlier-registered module's mappings to be
    /// silently discarded (only the last singleton registration wins).
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddConsumptionApplication(
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
            typeof(ValidationBehavior<,>));

        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        TypeAdapterConfig.GlobalSettings.Scan(assembly);
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
