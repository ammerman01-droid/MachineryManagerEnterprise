using MachineryManager.SharedKernel;

namespace MachineryManager.AuditLog.Application.Abstractions;

/// <summary>
/// Read-only access to the shared <c>audit.AuditEntry</c> table
/// (chat, 2026-09-05, gam 4). Deliberately exposes no Add/Update/Delete
/// members: the AuditLog module is strictly read-only.
/// </summary>
public interface IAuditEntryReadRepository
{
    /// <summary>
    /// Retrieves one page of audit records matching the given filter,
    /// ordered by <see cref="AuditEntry.OccurredAt"/> descending.
    /// </summary>
    /// <param name="filter">The filter and pagination parameters.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The matching records plus the total count across all pages.</returns>
    Task<(IReadOnlyList<AuditEntry> Items, int TotalCount)> SearchAsync(
        AuditEntrySearchFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>rieves a single audit record by its identifier, or <c>null</c> when no such record exists.</summary>
    Task<AuditEntry?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}