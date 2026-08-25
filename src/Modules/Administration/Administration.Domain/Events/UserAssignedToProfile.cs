using MachineryManager.SharedKernel;

namespace Administration.Domain.Events;

/// <summary>Raised when a User is assigned to a Profile at a specific scope.</summary>
public sealed class UserAssignedToProfile : IDomainEvent
{
    /// <summary>Gets the identifier of the assignment.</summary>
    public UserProfileAssignmentId AssignmentId { get; }

    /// <summary>Gets the identifier of the user.</summary>
    public Guid UserId { get; }

    /// <summary>Gets the identifier of the profile.</summary>
    public ProfileId ProfileId { get; }

    /// <summary>Gets the scope of the assignment.</summary>
    public AuthorizationScope Scope { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAssignedToProfile"/> class.
    /// </summary>
    /// <param name="assignmentId">The identifier of the assignment.</param>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="profileId">The identifier of the profile.</param>
    /// <param name="scope">The authorization scope.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public UserAssignedToProfile(
        UserProfileAssignmentId assignmentId,
        Guid userId,
        ProfileId profileId,
        AuthorizationScope scope,
        DateTimeOffset occurredOn)
    {
        AssignmentId = assignmentId;
        UserId = userId;
        ProfileId = profileId;
        Scope = scope;
        OccurredOn = occurredOn;
    }
}