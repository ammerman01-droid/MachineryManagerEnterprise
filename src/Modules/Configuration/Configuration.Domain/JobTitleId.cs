using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the strongly-typed identifier of a <see cref="JobTitle"/> aggregate.</summary>
public sealed class JobTitleId : ValueObject
{
    /// <summary>Gets the underlying identifier value.</summary>
    public Guid Value { get; }

    private JobTitleId(Guid value) => Value = value;

    /// <summary>Creates a new, unique identifier.</summary>
    public static JobTitleId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value.</summary>
    public static JobTitleId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}