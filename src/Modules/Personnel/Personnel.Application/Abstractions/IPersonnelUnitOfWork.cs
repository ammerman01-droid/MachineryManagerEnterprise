namespace MachineryManagerEnterprise.Personnel.Application.Abstractions;

/// <summary>Unit of Work contract for the Personnel module's persistence context (ADR-0006 — one DbContext per module).</summary>
public interface IPersonnelUnitOfWork
{
    /// <summary>Persists all pending changes and dispatches raised domain events.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}