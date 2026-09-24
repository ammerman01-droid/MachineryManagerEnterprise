using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application;

/// <summary>Wraps a Domain Event so it can be published through MediatR's in-process notification pipeline.</summary>
/// <typeparam name="TDomainEvent">The concrete Domain Event type.</typeparam>
public sealed class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    /// <summary>Gets the wrapped Domain Event.</summary>
    public TDomainEvent DomainEvent { get; }

    /// <summary>Initializes a new instance of the <see cref="DomainEventNotification{TDomainEvent}"/> class.</summary>
    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}
