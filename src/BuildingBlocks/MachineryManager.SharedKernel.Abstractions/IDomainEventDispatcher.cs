namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Dispatches domain events to their subscribers. Implemented in the
/// Application layer (ADR-0011) and consumed by Infrastructure to
/// publish events after successful persistence.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches the given domain events to all registered handlers.
    /// </summary>
    /// <param name="events">The domain events to dispatch.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task DispatchAsync(IReadOnlyCollection<IDomainEvent> events, CancellationToken cancellationToken = default);
}