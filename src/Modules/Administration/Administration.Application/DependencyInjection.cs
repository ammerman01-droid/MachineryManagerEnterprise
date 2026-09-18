using FluentValidation;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.Administration.Application;

/// <summary>Provides extension methods for registering Administration Application layer services.</summary>
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
/// module-local <c>new TypeAdapterConfig()</c>. Each module previously
/// registered its own instance as a DI singleton for the same service
/// type (<see cref="TypeAdapterConfig"/>); since .NET's DI container
/// resolves only the last singleton registered for a given type, every
/// module's mapping configuration except the last-registered one was
/// silently discarded, causing <c>Mapster.CompileException</c> at
/// projection time for any module registered earlier than the last.
/// Scanning into the single shared <see cref="TypeAdapterConfig.GlobalSettings"/>
/// instance from every module avoids this collision.
/// </remarks>
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

    TypeAdapterConfig.GlobalSettings.Scan(assembly);
    services.AddSingleton(TypeAdapterConfig.GlobalSettings);
    services.AddScoped<IMapper, ServiceMapper>();

    return services;
}
}