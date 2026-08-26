using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Strongly-typed identifier for an Asset Model.</summary>
public sealed class AssetModelId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private AssetModelId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique AssetModelId.</summary>
    public static AssetModelId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static AssetModelId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}