using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Maintenance.Domain.Events;

/// <summary>
/// Raised when a Work Order is converted to repair — the point at which
/// the (future) Repair Execution phase may begin recording the actual
/// work done against this Work Order.
/// </summary>
public sealed class WorkOrderConvertedToRepair : IDomainEvent
{
    /// <summary>Gets the identifier of the converted Work Order.</summary>
    public WorkOrderId WorkOrderId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the identifier of the Asset this Work Order was opened for.</summary>
    public Guid AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkOrderConvertedToRepair"/> class.</summary>
    public WorkOrderConvertedToRepair(WorkOrderId workOrderId, Guid organizationId, Guid assetId, DateTimeOffset occurredOn)
    {
        WorkOrderId = workOrderId;
        OrganizationId = organizationId;
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}
