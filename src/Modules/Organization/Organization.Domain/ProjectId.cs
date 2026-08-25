using MachineryManager.SharedKernel;

namespace Organization.Domain;

/// <summary>Strongly-typed identifier for a Project.</summary>
public sealed class ProjectId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private ProjectId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique ProjectId.</summary>
    public static ProjectId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static ProjectId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}