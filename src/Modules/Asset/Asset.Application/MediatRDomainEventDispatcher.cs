using System.Reflection;
using MediatR;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application;

/// <summary>
/// Dispatches domain events through MediatR as <see cref="INotification"/>
/// wrappers, bridging the SharedKernel abstraction to the Application-layer
/// messaging infrastructure (ADR-0011).
/// </summary>
public sealed class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatRDomainEventDispatcher"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR mediator.</param>
    public MediatRDomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task DispatchAsync(IReadOnlyCollection<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in events)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(notificationType, domainEvent)!;

            var method = typeof(MediatRDomainEventDispatcher)
                .GetMethod(nameof(PublishTyped), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(notificationType);

            await (Task)method.Invoke(this, new[] { notification, cancellationToken })!;
        }
    }

    /// <summary>
    /// Strongly-typed helper that satisfies MediatR's generic constraint
    /// on <see cref="INotification"/>.
    /// </summary>
    private async Task PublishTyped<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        await _mediator.Publish(notification, cancellationToken);
    }
}