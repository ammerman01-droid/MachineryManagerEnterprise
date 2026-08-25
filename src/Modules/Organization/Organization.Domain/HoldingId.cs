using MachineryManager.SharedKernel;

namespace Organization.Domain;

/// <summary>Strongly-typed identifier for a Holding (top-level tenant grouping).</summary>
public sealed class HoldingId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private HoldingId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique HoldingId.</summary>
    public static HoldingId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static HoldingId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}