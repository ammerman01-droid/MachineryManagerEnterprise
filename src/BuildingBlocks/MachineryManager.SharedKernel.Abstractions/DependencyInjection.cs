using MachineryManager.SharedKernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.SharedKernel.Infrastructure;

/// <summary>
/// Registers cross-cutting infrastructure services shared by every module.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the shared infrastructure implementations of the
    /// SharedKernel abstractions.
    /// </summary>
    /// <remarks>
    /// Only <see cref="IDateTimeProvider"/> is registered here.
    /// <see cref="ICurrentUserService"/> is intentionally NOT registered
    /// yet: no authentication mechanism exists until the Identity
    /// module (ADR-0030) is implemented. Registering a stub here would
    /// silently invent authentication behavior, which the AI Engineering
    /// Contract prohibits — this is an explicit open item, not an
    /// oversight.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddSharedKernelInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}