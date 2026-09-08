using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application;

/// <summary>
/// Dispatches domain events by publishing each one, wrapped in a
/// <see cref="DomainEventNotification{TDomainEvent}"/>, through
/// MediatR's in-process notification pipeline.
/// </summary>
public sealed class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;

    /// <summary>Initializes a new instance of the <see cref="MediatRDomainEventDispatcher"/> class.</summary>
    /// <param name="publisher">The MediatR publisher used to dispatch notifications.</param>
    public MediatRDomainEventDispatcher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    /// <summary>Dispatches the given domain events, one notification per event.</summary>
    /// <param name="domainEvents">The domain events to dispatch.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    public async Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;

            await _publisher.Publish(notification, cancellationToken);
        }
    }
}