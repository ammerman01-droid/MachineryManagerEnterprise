using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a new Job Title option is registered.</summary>
public sealed class JobTitleRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered Job Title.</summary>
    public JobTitleId JobTitleId { get; }

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the name of the Job Title.</summary>
    public string Name { get; }

    /// <summary>Gets the moment this event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="JobTitleRegistered"/> class.</summary>
    public JobTitleRegistered(JobTitleId jobTitleId, Guid holdingId, string name, DateTimeOffset occurredOn)
    {
        JobTitleId = jobTitleId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}