namespace MachineryManagerEnterprise.Identity.Infrastructure.Http;

/// <summary>
/// Exposes the DI scope of the Blazor circuit that is currently executing, so services
/// created outside that scope (such as HttpClientFactory message handlers) can still
/// resolve circuit-scoped services belonging to the correct user.
/// </summary>
public sealed class CircuitServicesAccessor
{
    private static readonly AsyncLocal<IServiceProvider?> BlazorServices = new();

    /// <summary>Gets the current circuit's service provider, or <see langword="null"/> outside a circuit.</summary>
    public IServiceProvider? Services
    {
        get => BlazorServices.Value;
        internal set => BlazorServices.Value = value;
    }
}