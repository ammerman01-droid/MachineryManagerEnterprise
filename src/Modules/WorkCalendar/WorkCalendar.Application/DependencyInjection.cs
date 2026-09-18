using FluentValidation;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mapster;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application;

/// <summary>
/// Provides extension methods for registering WorkCalendar Application layer
/// services into the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR handlers, FluentValidation validators, pipeline
    /// behaviors (Validation per ADR-0036), the domain event dispatcher,
    /// and the Mapster-backed mapper.
    /// </summary>
    /// <remarks>
    /// Fix (chat, 2026-09-14): this previously created a private
    /// <c>new TypeAdapterConfig()</c> and registered it as the DI
    /// singleton for <see cref="TypeAdapterConfig"/> — exactly the
    /// own remarks warn about. Since .NET DI resolves only the
    /// last-registered singleton for a given type, and this module is
    /// registered after Administration/Organization/etc. in
    /// <c>Program.cs</c>, every other module's scanned mappings were
    /// silently discarded, breaking <c>IMapper</c> project-wide (not
    /// just for WorkCalendar). Now scans into the shared
    /// <see cref="TypeAdapterConfig.GlobalSettings"/> instance like
    /// every other module.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddWorkCalendarApplication(
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
        services.AddScoped<WorkCalendarApplicationService>();

        return services;
    }
}