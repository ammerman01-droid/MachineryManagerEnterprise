namespace MachineryManager.SharedKernel;

/// <summary>
/// Base class for entities with identity-based equality.
/// Per ADR-0001 (Clean Architecture), entities never depend on
/// Infrastructure, Application, or Presentation types.
/// </summary>
/// <typeparam name="TId">The strongly-typed identifier type.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    /// <summary>The strongly-typed identifier of this entity.</summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the entity with the specified identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    // Reserved for ORM materialization only. Never used by application code.
    /// <summary>
    /// Initializes a new instance of the entity.
    /// </summary>
    protected Entity()
    {
        Id = default!;
    }

    /// <summary>Determines whether this entity has the same identity as <paramref name="other"/>.</summary>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    /// <summary>Determines whether two entities have the same identity.</summary>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) =>
        left is null ? right is null : left.Equals(right);

    /// <summary>Determines whether two entities have different identities.</summary>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
}