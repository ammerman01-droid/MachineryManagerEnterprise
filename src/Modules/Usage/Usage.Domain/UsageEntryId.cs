using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain;

/// <summary>Strongly-typed identifier for a single entry within a <see cref="UsageLedger"/>'s timeline.</summary>
public sealed class UsageEntryId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private UsageEntryId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique UsageEntryId.</summary>
    public static UsageEntryId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static UsageEntryId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
