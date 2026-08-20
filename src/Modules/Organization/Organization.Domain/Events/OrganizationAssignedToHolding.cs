using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when an Organization is assigned to a Holding.</summary>
public sealed class OrganizationAssignedToHolding : IDomainEvent
{
    /// <summary>Gets the identifier of the assigned organization.</summary>
    public OrganizationId OrganizationId { get; }

    /// <summary>Gets the identifier of the holding.</summary>
    public HoldingId HoldingId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="OrganizationAssignedToHolding"/> class.</summary>
    public OrganizationAssignedToHolding(OrganizationId organizationId, HoldingId holdingId, DateTimeOffset occurredOn)
    {
        OrganizationId = organizationId;
        HoldingId = holdingId;
        OccurredOn = occurredOn;
    }
}