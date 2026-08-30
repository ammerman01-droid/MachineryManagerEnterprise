using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the UnitOfMeasurementId type.</summary>
public sealed class UnitOfMeasurementId : ValueObject
{
    /// <summary>Gets the Value value.</summary>
    public Guid Value { get; }

    private UnitOfMeasurementId(Guid value) => Value = value;

    /// <summary>Executes the New operation.</summary>
    public static UnitOfMeasurementId New() => new(Guid.NewGuid());
    /// <summary>Executes the From operation.</summary>
    public static UnitOfMeasurementId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}