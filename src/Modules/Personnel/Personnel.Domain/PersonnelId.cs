using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Domain;

/// <summary>Represents the strongly-typed identifier of a <see cref="Personnel"/> aggregate.</summary>
public sealed class PersonnelId : ValueObject
{
    /// <summary>Gets the Value value.</summary>
    public Guid Value { get; }

    private PersonnelId(Guid value) => Value = value;

    /// <summary>Creates a new, unique identifier.</summary>
    public static PersonnelId New() => new(Guid.NewGuid());
    /// <summary>Wraps an existing identifier value.</summary>
    public static PersonnelId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}