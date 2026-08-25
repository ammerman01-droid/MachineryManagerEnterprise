using MachineryManager.SharedKernel;

namespace Organization.Domain.Events;

/// <summary>Raised when a Project's name is changed.</summary>
public sealed class ProjectRenamed : IDomainEvent
{
    /// <summary>Gets the identifier of the renamed project.</summary>
    public ProjectId ProjectId { get; }

    /// <summary>Gets the new name of the project.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="ProjectRenamed"/> class.</summary>
    public ProjectRenamed(ProjectId projectId, string name, DateTimeOffset occurredOn)
    {
        ProjectId = projectId;
        Name = name;
        OccurredOn = occurredOn;
    }
}