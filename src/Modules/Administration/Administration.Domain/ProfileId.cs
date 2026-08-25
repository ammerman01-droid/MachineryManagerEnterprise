using MachineryManager.SharedKernel;

namespace Administration.Domain;

/// <summary>Strongly-typed identifier for a Profile.</summary>
public sealed class ProfileId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private ProfileId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique ProfileId.</summary>
    public static ProfileId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value.</summary>
    public static ProfileId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}