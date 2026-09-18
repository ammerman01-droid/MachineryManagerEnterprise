using MediatR;
using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Application;

/// <summary>Wraps a domain event as a MediatR notification so it can be published through the pipeline.</summary>
public sealed class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    /// <summary>Gets the wrapped domain event.</summary>
    public TDomainEvent DomainEvent { get; }

    /// <summary>Initializes a new instance of the <see cref="DomainEventNotification{TDomainEvent}"/> class.</summary>
    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}