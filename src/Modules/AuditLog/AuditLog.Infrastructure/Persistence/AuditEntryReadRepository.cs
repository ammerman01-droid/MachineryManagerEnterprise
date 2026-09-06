using MachineryManager.AuditLog.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.AuditLog.Infrastructure.Persistence;

/// <inheritdoc cref="IAuditEntryReadRepository" />
public sealed class AuditEntryReadRepository : IAuditEntryReadRepository
{
    private readonly AuditLogDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="AuditEntryReadRepository"/> class.</summary>
    public AuditEntryReadRepository(AuditLogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<AuditEntry> Items, int TotalCount)> SearchAsync(
        AuditEntrySearchFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.AuditEntries
            .AsNoTracking();

        // gam 5 scope enforcement: only rows whose HoldingId OR
        // OrganizationId falls inside the caller's authorized scopes.
        // ProjectIds are intentionally not consulted — AuditEntry has
        // no ProjectId column (chat, 2026-09-06).
        if (filter.AuthorizedScope is { IsUnrestricted: false } scope)
        {
            var holdingIds = scope.HoldingIds;
            var organizationIds = scope.OrganizationIds;

            query = query.Where(entry =>
                (entry.HoldingId.HasValue && holdingIds.Contains(entry.HoldingId.Value)) ||
                (entry.OrganizationId.HasValue && organizationIds.Contains(entry.OrganizationId.Value)));
        }

        if (filter.From.HasValue)
        {
            query = query.Where(entry => entry.OccurredAt >= filter.From.Value);
        }

        if (filter.To.HasValue)
        {
            query = query.Where(entry => entry.OccurredAt <= filter.To.Value);
        }

        if (filter.UserId.HasValue)
        {
            query = query.Where(entry => entry.UserId == filter.UserId.Value);
        }

        if (filter.OperationType.HasValue)
        {
            query = query.Where(entry => entry.OperationType == filter.OperationType.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.SchemaName))
        {
            query = query.Where(entry => entry.SchemaName == filter.SchemaName);
        }

        if (!string.IsNullOrWhiteSpace(filter.TableName))
        {
            query = query.Where(entry => EF.Functions.Like(entry.TableName, $"%{filter.TableName}%"));
        }

        var orderedQuery = query
            .OrderByDescending(entry => entry.OccurredAt)
            .ThenByDescending(entry => entry.Id);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await orderedQuery
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc />
    public Task<AuditEntry?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        _dbContext.AuditEntries
            .AsNoTracking()
            .FirstOrDefaultAsync(entry => entry.Id == id, cancellationToken);
}