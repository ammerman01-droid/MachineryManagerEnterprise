using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>
/// Strongly-typed identifier for a Unit of Measurement.
/// </summary>
public sealed class UnitOfMeasurementId : ValueObject
{
    /// <summary>
    /// Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    private UnitOfMeasurementId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new, unique UnitOfMeasurementId.
    /// </summary>
    public static UnitOfMeasurementId New() => new(Guid.NewGuid());

    /// <summary>
    /// Wraps an existing identifier value, such as a value read from persistence.
    /// </summary>
    /// <param name="value">The underlying GUID value.</param>
    public static UnitOfMeasurementId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}