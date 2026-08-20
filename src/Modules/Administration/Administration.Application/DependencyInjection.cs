using FluentValidation;
using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.Administration.Application;

/// <summary>
/// Provides extension methods for registering Administration Application layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR, FluentValidation, pipeline behaviors, and domain event dispatcher.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAdministrationApplication(
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