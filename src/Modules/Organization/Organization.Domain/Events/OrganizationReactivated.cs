using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when a previously suspended Organization is reactivated.</summary>
public sealed class OrganizationReactivated : IDomainEvent
{
    /// <summary>Gets the identifier of the reactivated organization.</summary>
    public OrganizationId OrganizationId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="OrganizationReactivated"/> class.</summary>
    /// <param name="organizationId">The identifier of the reactivated organization.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public OrganizationReactivated(OrganizationId organizationId, DateTimeOffset occurredOn)
    {
        OrganizationId = organizationId;
        OccurredOn = occurredOn;
    }
}