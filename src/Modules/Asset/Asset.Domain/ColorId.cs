namespace Asset.Domain;

/// <summary>Strongly-typed identifier for the <see cref="Color"/> aggregate.</summary>
public sealed record ColorId
{
    /// <summary>Gets the underlying <see cref="Guid"/> value.</summary>
    public Guid Value { get; }

    private ColorId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique <see cref="ColorId"/>.</summary>
    public static ColorId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing <see cref="Guid"/> value as a <see cref="ColorId"/>.</summary>
    public static ColorId From(Guid value) => new(value);
}