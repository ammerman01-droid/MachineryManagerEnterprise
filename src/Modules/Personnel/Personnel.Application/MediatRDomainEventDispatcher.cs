using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application;

/// <summary>Dispatches raised domain events as MediatR notifications.</summary>
public sealed class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;

    /// <summary>Initializes a new instance of the <see cref="MediatRDomainEventDispatcher"/> class.</summary>
    public MediatRDomainEventDispatcher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    /// <summary>Publishes each domain event as a <see cref="DomainEventNotification{TDomainEvent}"/>.</summary>
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