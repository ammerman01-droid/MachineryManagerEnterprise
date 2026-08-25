using MediatR;
using MachineryManager.SharedKernel;

namespace MachineryManager.Administration.Application;

/// <summary>
/// Wraps a domain event as a MediatR <see cref="INotification"/>.
/// </summary>
/// <typeparam name="TDomainEvent">The type of the domain event being wrapped.</typeparam>
public sealed class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    /// <summary>Gets the underlying domain event.</summary>
    public TDomainEvent DomainEvent { get; }

    /// <summary>Initializes a new instance of the wrapper.</summary>
    /// <param name="domainEvent">The domain event to wrap.</param>
    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}