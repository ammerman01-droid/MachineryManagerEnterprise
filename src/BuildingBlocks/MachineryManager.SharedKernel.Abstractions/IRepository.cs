using MachineryManager.SharedKernel;

namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Base repository contract for an Aggregate Root. Per ADR-0006 (Use
/// Entity Framework Core), Aggregates are persisted through EF Core;
/// Dapper is used only for read-heavy queries per ADR-0019 and never
/// implements this interface.
/// </summary>
/// <typeparam name="TAggregate">The Aggregate Root type.</typeparam>
/// <typeparam name="TId">The Aggregate Root's strongly-typed identifier.</typeparam>
public interface IRepository<TAggregate, in TId>
    where TAggregate : AggregateRoot<TId>
    where TId : notnull
{
    /// <summary>Retrieves the Aggregate by its identifier, or null if it does not exist.</summary>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>Adds a newly created Aggregate to the repository.</summary>
    void Add(TAggregate aggregate);

    /// <summary>Marks an existing Aggregate as updated.</summary>
    void Update(TAggregate aggregate);

    /// <summary>
    /// Marks an existing Aggregate for permanent (hard) deletion. Callers
    /// are responsible for enforcing any business rule that must hold
    /// before a delete is allowed (e.g. no dependent records) — this
    /// method performs no such check itself.
    /// </summary>
    void Remove(TAggregate aggregate);
}