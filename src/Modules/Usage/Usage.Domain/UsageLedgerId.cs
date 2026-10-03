using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain;

/// <summary>Strongly-typed identifier for a Usage Ledger — one per (Owner, Unit).</summary>
public sealed class UsageLedgerId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private UsageLedgerId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique UsageLedgerId.</summary>
    public static UsageLedgerId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static UsageLedgerId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
