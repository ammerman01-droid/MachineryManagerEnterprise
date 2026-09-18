using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Domain;

/// <summary>Represents the strongly-typed identifier of a <see cref="PersonnelDrivingLicense"/> child entity.</summary>
public sealed class PersonnelDrivingLicenseId : ValueObject
{
    /// <summary>Gets the underlying identifier value.</summary>
    public Guid Value { get; }

    private PersonnelDrivingLicenseId(Guid value) => Value = value;

    /// <summary>Creates a new, unique identifier.</summary>
    public static PersonnelDrivingLicenseId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value.</summary>
    public static PersonnelDrivingLicenseId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}