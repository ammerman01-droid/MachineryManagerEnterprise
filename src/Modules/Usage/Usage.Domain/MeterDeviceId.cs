using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain;

/// <summary>Strongly-typed identifier for a Meter Device (BR-010 — independent from Operational Usage).</summary>
public sealed class MeterDeviceId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private MeterDeviceId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique MeterDeviceId.</summary>
    public static MeterDeviceId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static MeterDeviceId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
