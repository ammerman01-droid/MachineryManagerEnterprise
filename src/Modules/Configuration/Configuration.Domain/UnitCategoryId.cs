using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the UnitCategoryId type.</summary>
public sealed class UnitCategoryId : ValueObject
{
    /// <summary>Gets the Value value.</summary>
    public Guid Value { get; }

    private UnitCategoryId(Guid value) => Value = value;

    /// <summary>Executes the New operation.</summary>
    public static UnitCategoryId New() => new(Guid.NewGuid());
    /// <summary>Executes the From operation.</summary>
    public static UnitCategoryId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}