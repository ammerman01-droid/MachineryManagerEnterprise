using MachineryManager.SharedKernel;

namespace Administration.Domain.Events;

/// <summary>Raised when a new Profile is created.</summary>
public sealed class ProfileCreated : IDomainEvent
{
    /// <summary>Gets the identifier of the created profile.</summary>
    public ProfileId ProfileId { get; }

    /// <summary>Gets the name of the created profile.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileCreated"/> class.
    /// </summary>
    /// <param name="profileId">The identifier of the created profile.</param>
    /// <param name="name">The name of the created profile.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public ProfileCreated(ProfileId profileId, string name, DateTimeOffset occurredOn)
    {
        ProfileId = profileId;
        Name = name;
        OccurredOn = occurredOn;
    }
}