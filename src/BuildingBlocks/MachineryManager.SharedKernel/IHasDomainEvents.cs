namespace MachineryManager.SharedKernel;

/// <summary>
/// Implemented by aggregates that raise domain events, allowing
/// infrastructure to collect and dispatch them generically without
/// depending on specific aggregate types.
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>The domain events raised by this aggregate that have not yet been dispatched.</summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>Clears all domain events after they have been dispatched.</summary>
    void ClearDomainEvents();
}