using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Strongly-typed identifier for an Engine Model.</summary>
public sealed class EngineModelId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private EngineModelId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique EngineModelId.</summary>
    public static EngineModelId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static EngineModelId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}