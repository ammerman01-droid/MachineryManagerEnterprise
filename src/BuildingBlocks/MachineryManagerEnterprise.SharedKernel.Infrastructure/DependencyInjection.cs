using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.SharedKernel.Infrastructure;

/// <summary>
/// Registers cross-cutting infrastructure services shared by every module.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the shared infrastructure implementations of the
    /// SharedKernel abstractions.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddSharedKernelInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddHttpContextAccessor(); // از قبل در Program.cs هست
        services.AddScoped<IThemePreferenceStore, CookieThemePreferenceStore>();

        return services;
    }
}