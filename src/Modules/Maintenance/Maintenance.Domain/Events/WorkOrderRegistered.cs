using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Maintenance.Domain.Events;

/// <summary>Raised when a new Work Order is registered.</summary>
public sealed class WorkOrderRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered Work Order.</summary>
    public WorkOrderId WorkOrderId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the identifier of the Asset this Work Order was opened for.</summary>
    public Guid AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkOrderRegistered"/> class.</summary>
    public WorkOrderRegistered(WorkOrderId workOrderId, Guid organizationId, Guid assetId, DateTimeOffset occurredOn)
    {
        WorkOrderId = workOrderId;
        OrganizationId = organizationId;
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}
