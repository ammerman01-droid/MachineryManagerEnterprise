using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when a new Engine Model is registered.</summary>
public sealed class EngineModelRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered engine model.</summary>
    public EngineModelId EngineModelId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the name of the engine model.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="EngineModelRegistered"/> class.</summary>
    public EngineModelRegistered(EngineModelId engineModelId, Guid organizationId, string name, DateTimeOffset occurredOn)
    {
        EngineModelId = engineModelId;
        OrganizationId = organizationId;
        Name = name;
        OccurredOn = occurredOn;
    }
}