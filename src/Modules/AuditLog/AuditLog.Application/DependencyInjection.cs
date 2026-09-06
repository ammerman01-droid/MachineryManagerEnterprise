using FluentValidation;
using MachineryManager.AuditLog.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.AuditLog.Application;

/// <summary>Registers the AuditLog module's Application-layer services (chat, 2026-09-05, gam 4).</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR handlers, FluentValidation validators, and the
    /// validation pipeline behavior — mirroring the other business
    /// modules' Application-layer registration exactly (ADR-0036:
    /// validation runs in the pipeline; handlers do no ad hoc input
    /// validation).
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAuditLogApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}