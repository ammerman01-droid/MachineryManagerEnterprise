using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Maintenance.Domain.Events;

/// <summary>Raised when an open Work Order's details are edited.</summary>
public sealed class WorkOrderEdited : IDomainEvent
{
    /// <summary>Gets the identifier of the edited Work Order.</summary>
    public WorkOrderId WorkOrderId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="WorkOrderEdited"/> class.</summary>
    public WorkOrderEdited(WorkOrderId workOrderId, Guid organizationId, DateTimeOffset occurredOn)
    {
        WorkOrderId = workOrderId;
        OrganizationId = organizationId;
        OccurredOn = occurredOn;
    }
}
