using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when an Organization is suspended (BR-017, Section 10.16).</summary>
public sealed class OrganizationSuspended : IDomainEvent
{
    /// <summary>Gets the identifier of the suspended organization.</summary>
    public OrganizationId OrganizationId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="OrganizationSuspended"/> class.</summary>
    /// <param name="organizationId">The identifier of the suspended organization.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public OrganizationSuspended(OrganizationId organizationId, DateTimeOffset occurredOn)
    {
        OrganizationId = organizationId;
        OccurredOn = occurredOn;
    }
}