using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace MachineryManagerEnterprise.Identity.Infrastructure;

/// <summary>
/// Periodically deletes expired, redeemed and revoked OpenIddict tokens and orphaned
/// authorizations so the token tables do not grow without bound. Valid, unexpired
/// tokens are never removed.
/// </summary>
internal sealed class OpenIddictPruningService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);
    private static readonly TimeSpan Retention = TimeSpan.FromDays(14);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OpenIddictPruningService> _logger;

    /// <summary>Initializes a new instance of the <see cref="OpenIddictPruningService"/> class.</summary>
    /// <param name="scopeFactory">Creates a scope per pruning run (the managers are scoped).</param>
    /// <param name="logger">Diagnostic logger.</param>
    public OpenIddictPruningService(IServiceScopeFactory scopeFactory, ILogger<OpenIddictPruningService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);

        try
        {
            do
            {
                try
                {
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var threshold = DateTimeOffset.UtcNow - Retention;

                    await scope.ServiceProvider.GetRequiredService<IOpenIddictTokenManager>()
                        .PruneAsync(threshold, stoppingToken);
                    await scope.ServiceProvider.GetRequiredService<IOpenIddictAuthorizationManager>()
                        .PruneAsync(threshold, stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogWarning(ex, "OpenIddict pruning run failed; it will be retried at the next interval.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException)
        {
            // Host is shutting down.
        }
    }
}