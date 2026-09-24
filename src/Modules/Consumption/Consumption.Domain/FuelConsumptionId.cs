using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Consumption.Domain;

/// <summary>Strongly-typed identifier for a FuelConsumption record.</summary>
public sealed class FuelConsumptionId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private FuelConsumptionId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique FuelConsumptionId.</summary>
    public static FuelConsumptionId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static FuelConsumptionId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
