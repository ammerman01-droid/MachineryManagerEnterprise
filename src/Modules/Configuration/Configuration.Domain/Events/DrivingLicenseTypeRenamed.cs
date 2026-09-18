using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Driving License Type's name is changed.</summary>
public sealed class DrivingLicenseTypeRenamed : IDomainEvent
{
    /// <summary>Gets the identifier of the renamed Driving License Type.</summary>
    public DrivingLicenseTypeId DrivingLicenseTypeId { get; }

    /// <summary>Gets the new name.</summary>
    public string NewName { get; }

    /// <summary>Gets the moment this event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="DrivingLicenseTypeRenamed"/> class.</summary>
    public DrivingLicenseTypeRenamed(DrivingLicenseTypeId drivingLicenseTypeId, string newName, DateTimeOffset occurredOn)
    {
        DrivingLicenseTypeId = drivingLicenseTypeId;
        NewName = newName;
        OccurredOn = occurredOn;
    }
}