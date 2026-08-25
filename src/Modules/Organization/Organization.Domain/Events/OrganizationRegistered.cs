using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>
/// Raised when a new Organization is registered (UC-1301 / CMD-950).
/// </summary>
public sealed class OrganizationRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered organization.</summary>
    public OrganizationId OrganizationId { get; }

    /// <summary>Gets the name of the registered organization.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationRegistered"/> class.
    /// </summary>
    /// <param name="organizationId">The identifier of the registered organization.</param>
    /// <param name="name">The name of the registered organization.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public OrganizationRegistered(OrganizationId organizationId, string name, DateTimeOffset occurredOn)
    {
        OrganizationId = organizationId;
        Name = name;
        OccurredOn = occurredOn;
    }
}