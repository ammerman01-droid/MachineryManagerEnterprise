namespace MachineryManagerEnterprise.Maintenance.Application.Abstractions;

/// <summary>
/// Generates the next sequential, human-readable Work Order number
/// within an Organization, starting at 1 (chat, 2026-09-22 — confirmed
/// necessary). Implemented in Infrastructure using an atomic,
/// concurrency-safe counter, kept separate from the WorkOrder
/// aggregate's repository since it is a cross-cutting numbering
/// concern, not aggregate persistence.
/// </summary>
public interface IWorkOrderNumberGenerator
{
    /// <summary>
    /// Atomically reserves and returns the next Work Order number for
    /// the given Organization.
    /// </summary>
    /// <param name="organizationId">The Organization to generate the next number for.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The next sequential number, starting at 1 for an Organization's first Work Order.</returns>
    Task<int> GetNextNumberAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
