using FluentValidation;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.Asset.Application;

/// <summary>
/// Provides extension methods for registering Asset Application layer
/// services into the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR handlers, FluentValidation validators, pipeline
    /// behaviors (Validation per ADR-0036), and the domain event dispatcher.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAssetApplication(
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

        return services;
    }
}