using MachineryManager.SharedKernel;

namespace Organization.Domain;

/// <summary>Strongly-typed identifier for an Organization (GL-ORG-001).</summary>
public sealed class OrganizationId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private OrganizationId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique OrganizationId.</summary>
    /// <returns>A new <see cref="OrganizationId"/> instance.</returns>
    public static OrganizationId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    /// <param name="value">The existing GUID value.</param>
    /// <returns>An <see cref="OrganizationId"/> instance wrapping the provided value.</returns>
    public static OrganizationId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}