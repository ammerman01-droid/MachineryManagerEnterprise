using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Represents the DrivingLicenseTypeRegistered type.</summary>
public sealed class DrivingLicenseTypeRegistered : IDomainEvent
{
    /// <summary>Gets the DrivingLicenseTypeId value.</summary>
    public DrivingLicenseTypeId DrivingLicenseTypeId { get; }
    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }
    /// <summary>Gets the Name value.</summary>
    public string Name { get; }
    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="DrivingLicenseTypeRegistered"/> class.</summary>
    public DrivingLicenseTypeRegistered(DrivingLicenseTypeId drivingLicenseTypeId, Guid holdingId, string name, DateTimeOffset occurredOn)
    {
        DrivingLicenseTypeId = drivingLicenseTypeId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}