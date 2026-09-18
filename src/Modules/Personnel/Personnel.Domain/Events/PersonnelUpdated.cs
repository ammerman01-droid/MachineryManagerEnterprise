using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Domain.Events;

/// <summary>Raised when a Personnel record's editable details are changed.</summary>
public sealed class PersonnelUpdated : IDomainEvent
{
    /// <summary>Gets the identifier of the updated Personnel record.</summary>
    public PersonnelId PersonnelId { get; }

    /// <summary>Gets the moment this event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="PersonnelUpdated"/> class.</summary>
    public PersonnelUpdated(PersonnelId personnelId, DateTimeOffset occurredOn)
    {
        PersonnelId = personnelId;
        OccurredOn = occurredOn;
    }
}