using MachineryManagerEnterprise.UI.Theming;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.UI;

/// <summary>
/// Registers the services defined in <c>MachineryManagerEnterprise.UI</c>
/// (currently just the theming subsystem) with the DI container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds <see cref="ThemeService"/> as scoped (one instance per circuit)
    /// so it can be injected into <c>MainLayout</c> and any theme-picker
    /// component. Requires <c>IThemePreferenceStore</c> to already be
    /// registered — see <c>AddSharedKernelInfrastructure</c>.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    public static IServiceCollection AddMachineryManagerUiTheming(this IServiceCollection services)
    {
        services.AddScoped<ThemeService>();
        return services;
    }
}