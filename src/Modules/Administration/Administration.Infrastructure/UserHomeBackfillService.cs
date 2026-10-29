using MachineryManagerEnterprise.Administration.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MachineryManagerEnterprise.Administration.Infrastructure;

/// <summary>
/// One-time, idempotent migration of existing users: every user that has a non-Platform
/// assignment but no <c>UserHome</c> yet gets a home derived from its most recent such assignment,
/// so Holding/Organization administrators can see the users created before homes existed.
/// Runs at startup; a no-op once every such user has a home.
/// </summary>
internal sealed class UserHomeBackfillService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UserHomeBackfillService> _logger;

    /// <summary>Initializes a new instance of the <see cref="UserHomeBackfillService"/> class.</summary>
    /// <param name="scopeFactory">Creates the scope the repositories live in.</param>
    /// <param name="logger">Diagnostic logger.</param>
    public UserHomeBackfillService(IServiceScopeFactory scopeFactory, ILogger<UserHomeBackfillService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Persistence.AdministrationDbContext>();
            var chainResolver = scope.ServiceProvider.GetRequiredService<IScopeChainResolver>();

            var homed = await dbContext.Set<global::Administration.Domain.UserHome>()
                .Select(home => home.UserId)
                .ToListAsync(stoppingToken);

            var candidates = (await dbContext.UserProfileAssignments
                    .AsNoTracking()
                    .ToListAsync(stoppingToken))
                .Where(a => a.Scope.Level != global::Administration.Domain.AuthorizationScopeLevel.Platform
                    && !homed.Contains(a.UserId))
                .GroupBy(a => a.UserId)
                .Select(group => group
                    .OrderByDescending(a => a.IsActive)
                    .ThenByDescending(a => a.LastChangedAt ?? a.AssignedAt)
                    .First())
                .ToList();

            var created = 0;

            foreach (var assignment in candidates)
            {
                var chain = await chainResolver.ResolveAsync(assignment.Scope, stoppingToken);

                if (chain.IsFailure)
                {
                    _logger.LogWarning(
                        "Could not derive a home for user {UserId}: {Error}",
                        assignment.UserId,
                        chain.Error.Message);
                    continue;
                }

                dbContext.Set<global::Administration.Domain.UserHome>().Add(
                    global::Administration.Domain.UserHome.Create(
                        assignment.UserId,
                        chain.Value.HoldingId,
                        chain.Value.OrganizationId,
                        chain.Value.ProjectId,
                        DateTimeOffset.UtcNow));
                created++;
            }

            if (created > 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Backfilled the home scope of {Count} existing users.", created);
            }
        }
        catch (OperationCanceledException)
        {
            // Host is shutting down.
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "User-home backfill failed; it will be retried at the next startup.");
        }
    }
}
