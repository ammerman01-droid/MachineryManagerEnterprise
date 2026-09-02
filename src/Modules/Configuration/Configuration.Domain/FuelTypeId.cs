using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Strongly-typed identifier for a <see cref="FuelType"/>.</summary>
public sealed class FuelTypeId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private FuelTypeId(Guid value) => Value = value;

    /// <summary>Creates a new, unique FuelTypeId.</summary>
    public static FuelTypeId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static FuelTypeId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}