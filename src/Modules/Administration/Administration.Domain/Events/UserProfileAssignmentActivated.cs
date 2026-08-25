using MachineryManager.SharedKernel;

namespace Administration.Domain.Events;

/// <summary>
/// Raised when a previously inactive UserProfileAssignment is
/// (re)activated (chat, 2026-08-25 — revised).
/// </summary>
public sealed class UserProfileAssignmentActivated : IDomainEvent
{
    /// <summary>Gets the identifier of the activated assignment.</summary>
    public UserProfileAssignmentId AssignmentId { get; }

    /// <summary>Gets the identifier of the affected user.</summary>
    public Guid UserId { get; }

    /// <summary>Gets the identifier of the profile that was assigned.</summary>
    public ProfileId ProfileId { get; }

    /// <summary>Gets the scope the assignment applies to.</summary>
    public AuthorizationScope Scope { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfileAssignmentActivated"/> class.
    /// </summary>
    /// <param name="assignmentId">The identifier of the activated assignment.</param>
    /// <param name="userId">The identifier of the affected user.</param>
    /// <param name="profileId">The identifier of the profile that was assigned.</param>
    /// <param name="scope">The scope the assignment applies to.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public UserProfileAssignmentActivated(
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