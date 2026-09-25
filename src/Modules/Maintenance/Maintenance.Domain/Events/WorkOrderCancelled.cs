using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Maintenance.Domain.Events;

/// <summary>Raised when a Work Order is cancelled.</summary>
public sealed class WorkOrderCancelled : IDomainEvent
{
    /// <summary>Gets the identifier of the cancelled Work Order.</summary>
    public WorkOrderId WorkOrderId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the reason given for the cancellation.</summary>
    public string Reason { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkOrderCancelled"/> class.</summary>
    public WorkOrderCancelled(WorkOrderId workOrderId, Guid organizationId, string reason, DateTimeOffset occurredOn)
    {
        WorkOrderId = workOrderId;
        OrganizationId = organizationId;
        Reason = reason;
        OccurredOn = occurredOn;
    }
}
