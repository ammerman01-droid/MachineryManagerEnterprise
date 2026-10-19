using Microsoft.AspNetCore.Components.Server.Circuits;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Http;

/// <summary>
/// Publishes the circuit's service provider to <see cref="CircuitServicesAccessor"/>
/// for the duration of every inbound circuit activity.
/// </summary>
internal sealed class ServicesAccessorCircuitHandler : CircuitHandler
{
    private readonly IServiceProvider _services;
    private readonly CircuitServicesAccessor _circuitServicesAccessor;

    /// <summary>Initializes a new instance of the <see cref="ServicesAccessorCircuitHandler"/> class.</summary>
    /// <param name="services">The circuit-scoped service provider.</param>
    /// <param name="circuitServicesAccessor">The shared accessor to publish it to.</param>
    public ServicesAccessorCircuitHandler(IServiceProvider services, CircuitServicesAccessor circuitServicesAccessor)
    {
        _services = services;
        _circuitServicesAccessor = circuitServicesAccessor;
    }

    /// <inheritdoc />
    public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
        Func<CircuitInboundActivityContext, Task> next) =>
        async context =>
        {
            _circuitServicesAccessor.Services = _services;
            await next(context);
            _circuitServicesAccessor.Services = null;
        };
}