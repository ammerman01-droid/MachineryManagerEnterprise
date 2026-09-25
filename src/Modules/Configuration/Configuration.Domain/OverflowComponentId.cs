using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the strongly-typed identifier of an <see cref="OverflowComponent"/> aggregate.</summary>
public sealed class OverflowComponentId : ValueObject
{
    /// <summary>Gets the underlying identifier value.</summary>
    public Guid Value { get; }

    private OverflowComponentId(Guid value) => Value = value;

    /// <summary>Creates a new, unique identifier.</summary>
    public static OverflowComponentId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value.</summary>
    public static OverflowComponentId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
