namespace WorkCalendar.Domain;

/// <summary>Strongly-typed identifier for the <see cref="DayOverride"/> entity.</summary>
public sealed record DayOverrideId
{
    /// <summary>Gets the underlying <see cref="Guid"/> value.</summary>
    public Guid Value { get; }

    private DayOverrideId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique <see cref="DayOverrideId"/>.</summary>
    public static DayOverrideId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing <see cref="Guid"/> value as a <see cref="DayOverrideId"/>.</summary>
    public static DayOverrideId From(Guid value) => new(value);
}