namespace MachineryManager.SharedKernel;

/// <summary>
/// Base class for Value Objects: immutable types compared by their
/// component values rather than by identity.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Returns the components that define structural equality for this
    /// Value Object, in a stable, deterministic order.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>Determines whether this Value Object has the same components as <paramref name="other"/>.</summary>
    public bool Equals(ValueObject? other)
    {
        if (other is null || GetType() != other.GetType())
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ValueObject);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var component in GetEqualityComponents())
        {
            hash.Add(component);
        }

        return hash.ToHashCode();
    }

    /// <summary>Determines whether two Value Objects have the same components.</summary>
    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        left is null ? right is null : left.Equals(right);

    /// <summary>Determines whether two Value Objects have different components.</summary>
    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}