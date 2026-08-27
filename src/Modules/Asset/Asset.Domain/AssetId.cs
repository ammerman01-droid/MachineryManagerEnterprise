using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Strongly-typed identifier for an Asset (BR-001 — a permanent identity).</summary>
public sealed class AssetId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private AssetId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique AssetId.</summary>
    public static AssetId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static AssetId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}