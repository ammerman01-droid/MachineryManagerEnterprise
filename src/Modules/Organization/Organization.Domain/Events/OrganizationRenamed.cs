using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when an Organization's name is changed.</summary>
public sealed class OrganizationRenamed : IDomainEvent
{
    /// <summary>Gets the identifier of the renamed organization.</summary>
    public OrganizationId OrganizationId { get; }

    /// <summary>Gets the new name of the organization.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="OrganizationRenamed"/> class.</summary>
    public OrganizationRenamed(OrganizationId organizationId, string name, DateTimeOffset occurredOn)
    {
        OrganizationId = organizationId;
        Name = name;
        OccurredOn = occurredOn;
    }
}