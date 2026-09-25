using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Maintenance.Infrastructure;

/// <summary>
/// <see cref="IWorkOrderNumberGenerator"/> implementation backed by a
/// dedicated <see cref="WorkOrderSequence"/> counter row per
/// Organization, updated atomically to stay correct under concurrent
/// registrations (chat, 2026-09-22 — a plain in-memory MAX(Number)+1
/// query would race between two simultaneous requests).
/// </summary>
public sealed class WorkOrderNumberGenerator : IWorkOrderNumberGenerator
{
    private const int PrimaryKeyViolationErrorNumber = 2627;
    private const int UniqueIndexViolationErrorNumber = 2601;

    private readonly MaintenanceDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="WorkOrderNumberGenerator"/> class.</summary>
    /// <param name="dbContext">The Maintenance module's persistence context.</param>
    public WorkOrderNumberGenerator(MaintenanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<int> GetNextNumberAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var incremented = await TryIncrementExistingAsync(organizationId, cancellationToken);
        if (incremented is { } value)
        {
            return value;
        }

        // No counter row yet for this Organization — this is its first
        // Work Order. Insert the starting row directly (bypassing
        // change tracking, so it commits immediately and independently
        // of the caller's own SaveChangesAsync for the WorkOrder
        // itself).
        try
        {
            await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO maintenance.WorkOrderSequence (OrganizationId, LastNumber) VALUES ({organizationId}, 1)",
                cancellationToken);

            return 1;
        }
        catch (SqlException ex) when (IsUniqueViolation(ex))
        {
            // Lost a race with a concurrent first-registration for the
            // same Organization — the row now exists, so increment it
            // instead.
            var retried = await TryIncrementExistingAsync(organizationId, cancellationToken);
            return retried ?? throw new InvalidOperationException(
                $"Failed to generate a Work Order number for Organization {organizationId} after a concurrent insert.");
        }
    }

    private async Task<int?> TryIncrementExistingAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        var results = await _dbContext.Database
            .SqlQueryRaw<int>(
                """
                UPDATE maintenance.WorkOrderSequence
                SET LastNumber = LastNumber + 1
                OUTPUT INSERTED.LastNumber
                WHERE OrganizationId = {0}
                """,
                organizationId)
            .ToListAsync(cancellationToken);

        return results.Count > 0 ? results[0] : null;
    }

    private static bool IsUniqueViolation(SqlException ex) =>
        ex.Number == PrimaryKeyViolationErrorNumber || ex.Number == UniqueIndexViolationErrorNumber;
}
