namespace WorkCalendar.Domain;

/// <summary>Strongly-typed identifier for the <see cref="WorkCalendar"/> aggregate.</summary>
public sealed record WorkCalendarId
{
    /// <summary>Gets the underlying <see cref="Guid"/> value.</summary>
    public Guid Value { get; }

    private WorkCalendarId(Guid value)
    {
        Value = value;
    }

    /// <summary>Creates a new, unique <see cref="WorkCalendarId"/>.</summary>
    public static WorkCalendarId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing <see cref="Guid"/> value as a <see cref="WorkCalendarId"/>.</summary>
    public static WorkCalendarId From(Guid value) => new(value);
}