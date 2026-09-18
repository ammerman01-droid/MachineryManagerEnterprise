using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Domain;

/// <summary>
/// Child entity of the <see cref="Personnel"/> aggregate representing a
/// single driving license held by that Personnel. A Personnel may hold
/// zero, one, or multiple licenses — at most one per Driving License Type
/// (enforced by <see cref="Personnel.AddDrivingLicense"/>).
/// </summary>
public sealed class PersonnelDrivingLicense : Entity<PersonnelDrivingLicenseId>
{
    /// <summary>Gets the identifier of the Driving License Type (managed in the Configuration module).</summary>
    public Guid DrivingLicenseTypeId { get; private set; }

    /// <summary>Gets the expiry date of this license.</summary>
    public DateOnly ExpiryDate { get; private set; }

    private PersonnelDrivingLicense()
    {
    }

    private PersonnelDrivingLicense(PersonnelDrivingLicenseId id, Guid drivingLicenseTypeId, DateOnly expiryDate)
        : base(id)
    {
        DrivingLicenseTypeId = drivingLicenseTypeId;
        ExpiryDate = expiryDate;
    }

    /// <summary>
    /// Creates a new <see cref="PersonnelDrivingLicense"/> instance.
    /// Validation of inputs is the caller's (<see cref="Personnel"/>)
    /// responsibility — this factory is intentionally internal.
    /// </summary>
    internal static PersonnelDrivingLicense Create(Guid drivingLicenseTypeId, DateOnly expiryDate) =>
        new(PersonnelDrivingLicenseId.New(), drivingLicenseTypeId, expiryDate);
}