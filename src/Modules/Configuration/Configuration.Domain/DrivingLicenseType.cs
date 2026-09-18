using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a manageable driving-license-type option
/// (e.g. "Class B", "Class C+E"), used by the Personnel module. Lives in
/// the Configuration module as reference/master data (Section 5.2), the
/// same way Color/FuelType/UnitOfMeasurement do — Holding-scoped so every
/// Organization under the same Holding shares one catalog.
/// </summary>
public sealed class DrivingLicenseType : AggregateRoot<DrivingLicenseTypeId>
{
    /// <summary>Gets the MaxNameLength constant.</summary>
    public const int MaxNameLength = 50;

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; private set; }
    /// <summary>Gets the Name value.</summary>
    public string Name { get; private set; } = string.Empty;

    private DrivingLicenseType()
    {
    }

    private DrivingLicenseType(DrivingLicenseTypeId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

    /// <summary>Executes the Register operation.</summary>
    public static Result<DrivingLicenseType> Register(Guid holdingId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<DrivingLicenseType>(DrivingLicenseTypeErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<DrivingLicenseType>(DrivingLicenseTypeErrors.NameTooLong(MaxNameLength));
        }

        var drivingLicenseType = new DrivingLicenseType(DrivingLicenseTypeId.New(), holdingId, name.Trim());

        drivingLicenseType.RaiseDomainEvent(new DrivingLicenseTypeRegistered(
            drivingLicenseType.Id, holdingId, drivingLicenseType.Name, dateTimeProvider.UtcNow));

        return drivingLicenseType;
    }

    /// <summary>Renames this Driving License Type.</summary>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(DrivingLicenseTypeErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(DrivingLicenseTypeErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();
        RaiseDomainEvent(new Events.DrivingLicenseTypeRenamed(Id, Name, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}