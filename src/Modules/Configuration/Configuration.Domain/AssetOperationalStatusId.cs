using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Strongly-typed identifier for the <see cref="AssetOperationalStatus"/> aggregate.</summary>
public sealed class AssetOperationalStatusId : ValueObject
{
    /// <summary>Gets the underlying <see cref="Guid"/> value.</summary>
    public Guid Value { get; }

    private AssetOperationalStatusId(Guid value) => Value = value;

    /// <summary>Creates a new, random identifier.</summary>
    public static AssetOperationalStatusId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing <see cref="Guid"/> value.</summary>
    public static AssetOperationalStatusId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}