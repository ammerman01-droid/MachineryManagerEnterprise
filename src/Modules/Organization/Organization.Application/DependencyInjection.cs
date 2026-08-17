using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.Organization.Application;

/// <summary>
/// Provides extension methods for registering Organization Application layer
/// services into the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR handlers, FluentValidation validators, and pipeline
    /// behaviors (Validation per ADR-0036) from the Organization.Application assembly.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddOrganizationApplication(
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

        return services;
    }
}