using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the ColorId type.</summary>
public sealed class ColorId : ValueObject
{
    /// <summary>Gets the Value value.</summary>
    public Guid Value { get; }

    private ColorId(Guid value) => Value = value;

    /// <summary>Executes the New operation.</summary>
    public static ColorId New() => new(Guid.NewGuid());
    /// <summary>Executes the From operation.</summary>
    public static ColorId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}