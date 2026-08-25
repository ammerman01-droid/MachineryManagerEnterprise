using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when a new Project is registered under an Organization.</summary>
public sealed class ProjectRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered project.</summary>
    public ProjectId ProjectId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public OrganizationId OrganizationId { get; }

    /// <summary>Gets the name of the registered project.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="ProjectRegistered"/> class.</summary>
    public ProjectRegistered(ProjectId projectId, OrganizationId organizationId, string name, DateTimeOffset occurredOn)
    {
        ProjectId = projectId;
        OrganizationId = organizationId;
        Name = name;
        OccurredOn = occurredOn;
    }
}