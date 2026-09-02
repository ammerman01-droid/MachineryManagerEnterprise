using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>
/// Strongly-typed identifier for a Company.
/// </summary>
public sealed class CompanyId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private CompanyId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new unique Company identifier.</summary>
    public static CompanyId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing Company identifier.</summary>
    /// <param name="value">The existing identifier value.</param>
    /// <returns>A <see cref="CompanyId"/> instance.</returns>
    public static CompanyId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}