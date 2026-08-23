using MachineryManager.SharedKernel;

namespace Administration.Domain.Events;

/// <summary>Raised when a UserProfileAssignment is revoked (BR-017, Access revocation).</summary>
public sealed class UserProfileAssignmentRevoked : IDomainEvent
{
    /// <summary>Gets the identifier of the revoked assignment.</summary>
    public UserProfileAssignmentId AssignmentId { get; }

    /// <summary>Gets the identifier of the affected user.</summary>
    public Guid UserId { get; }

    /// <summary>Gets the identifier of the profile that was assigned.</summary>
    public ProfileId ProfileId { get; }

    /// <summary>Gets the scope the assignment applied to.</summary>
    public AuthorizationScope Scope { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfileAssignmentRevoked"/> class.
    /// </summary>
    /// <param name="assignmentId">The identifier of the revoked assignment.</param>
    /// <param name="userId">The identifier of the affected user.</param>
    /// <param name="profileId">The identifier of the profile that was assigned.</param>
    /// <param name="scope">The scope the assignment applied to.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public UserProfileAssignmentRevoked(
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