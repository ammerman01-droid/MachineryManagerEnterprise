namespace MachineryManager.SharedKernel;

/// <summary>
/// Base class for Aggregate Roots, per docs-english/03-domain/05-Aggregates.md.
/// An Aggregate Root is the only entry point for modifying its
/// Aggregate; it is the sole publisher of the Domain Events that
/// describe the business facts resulting from its own operations.
/// </summary>
/// <typeparam name="TId">The strongly-typed identifier type.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Initializes a new instance of the aggregate root with the specified identifier.
    /// </summary>
    /// <param name="id">The aggregate identifier.</param>
    protected AggregateRoot(TId id)
        : base(id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the aggregate root.
    /// </summary>
    protected AggregateRoot()
    {
    }

    /// <summary>Domain Events raised by this Aggregate Root that have not yet been dispatched.</summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Registers a domain event raised by this aggregate.
    /// </summary>
    /// <param name="domainEvent">The domain event to register.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Called by Infrastructure (e.g. the EF Core SaveChanges pipeline)
    /// after Domain Events have been dispatched. Application and
    /// Domain code shall never call this directly.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}