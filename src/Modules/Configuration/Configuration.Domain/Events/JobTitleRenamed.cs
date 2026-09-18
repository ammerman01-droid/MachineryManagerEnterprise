using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Job Title's name is changed.</summary>
public sealed class JobTitleRenamed : IDomainEvent
{
    /// <summary>Gets the identifier of the renamed Job Title.</summary>
    public JobTitleId JobTitleId { get; }

    /// <summary>Gets the new name.</summary>
    public string NewName { get; }

    /// <summary>Gets the moment this event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="JobTitleRenamed"/> class.</summary>
    public JobTitleRenamed(JobTitleId jobTitleId, string newName, DateTimeOffset occurredOn)
    {
        JobTitleId = jobTitleId;
        NewName = newName;
        OccurredOn = occurredOn;
    }
}