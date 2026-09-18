using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Domain.Events;

/// <summary>Raised when a new Personnel record is registered.</summary>
public sealed class PersonnelRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered Personnel.</summary>
    public PersonnelId PersonnelId { get; }

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the identifier of the Project this Personnel was initially assigned to.</summary>
    public Guid ProjectId { get; }

    /// <summary>Gets the personnel code.</summary>
    public string PersonnelCode { get; }

    /// <summary>Gets the moment this event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="PersonnelRegistered"/> class.</summary>
    public PersonnelRegistered(PersonnelId personnelId, Guid organizationId, Guid projectId, string personnelCode, DateTimeOffset occurredOn)
    {
        PersonnelId = personnelId;
        OrganizationId = organizationId;
        ProjectId = projectId;
        PersonnelCode = personnelCode;
        OccurredOn = occurredOn;
    }
}