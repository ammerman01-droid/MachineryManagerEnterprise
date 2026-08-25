namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Commits the changes made within the current business operation as a
/// single atomic unit. Implemented in Infrastructure (EF Core), per
/// ADR-0006 and ADR-0019 (Hybrid Persistence Strategy).
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Persists all pending changes and dispatches any raised Domain Events.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}