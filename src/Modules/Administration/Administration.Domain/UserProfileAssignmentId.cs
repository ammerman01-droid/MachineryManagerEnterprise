using MachineryManager.SharedKernel;

namespace Administration.Domain;

/// <summary>Strongly-typed identifier for a UserProfileAssignment.</summary>
public sealed class UserProfileAssignmentId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private UserProfileAssignmentId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique UserProfileAssignmentId.</summary>
    public static UserProfileAssignmentId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value.</summary>
    public static UserProfileAssignmentId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}