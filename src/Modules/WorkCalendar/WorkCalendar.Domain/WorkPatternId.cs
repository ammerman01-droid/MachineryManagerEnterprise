namespace WorkCalendar.Domain;

/// <summary>Strongly-typed identifier for the <see cref="WorkPattern"/> entity.</summary>
public sealed record WorkPatternId
{
    /// <summary>Gets the underlying <see cref="Guid"/> value.</summary>
    public Guid Value { get; }

    private WorkPatternId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique <see cref="WorkPatternId"/>.</summary>
    public static WorkPatternId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing <see cref="Guid"/> value as a <see cref="WorkPatternId"/>.</summary>
    public static WorkPatternId From(Guid value) => new(value);
}