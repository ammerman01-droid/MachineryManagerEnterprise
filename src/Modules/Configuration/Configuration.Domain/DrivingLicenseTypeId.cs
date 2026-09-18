using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the DrivingLicenseTypeId type.</summary>
public sealed class DrivingLicenseTypeId : ValueObject
{
    /// <summary>Gets the Value value.</summary>
    public Guid Value { get; }

    private DrivingLicenseTypeId(Guid value) => Value = value;

    /// <summary>Executes the New operation.</summary>
    public static DrivingLicenseTypeId New() => new(Guid.NewGuid());
    /// <summary>Executes the From operation.</summary>
    public static DrivingLicenseTypeId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}