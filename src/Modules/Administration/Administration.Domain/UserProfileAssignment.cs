using Administration.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Administration.Domain;

/// <summary>
/// Represents the assignment of a <see cref="Profile"/> to a User at a
/// specific <see cref="AuthorizationScope"/> (Section 5.8). A user may
/// keep many assignments in their list over time, but at most one may be
/// <see cref="IsActive"/> at any moment (chat, 2026-08-25 — revised).
/// Unlike the project's default historical-integrity rules, this
/// aggregate intentionally allows an assignment to toggle between active
/// and inactive any number of times (explicit product decision, chat,
/// 2026-08-25 — revised): "Active" and "Inactive" are reversible states
/// of the same record, not a one-way audit trail.
/// </summary>
public sealed class UserProfileAssignment : AggregateRoot<UserProfileAssignmentId>
{
    /// <summary>Gets the identifier of the User being assigned.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Gets the identifier of the Profile being assigned.</summary>
    public ProfileId ProfileId { get; private set; }

    /// <summary>Gets the scope at which the assignment applies.</summary>
    public AuthorizationScope Scope { get; private set; }

    /// <summary>Gets the UTC timestamp when the assignment was created.</summary>
    public DateTimeOffset AssignedAt { get; private set; }

    /// <summary>
    /// Gets whether this assignment is currently the user's active
    /// Profile. Exactly one assignment per user may be active at a time;
    /// this is enforced at the application layer (chat, 2026-08-25 —
    /// revised) by the command handlers that assign or activate a
    /// Profile, not by this aggregate alone (each handler orchestrates
    /// across multiple aggregate instances for the same user).
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the UTC timestamp of the last activation or deactivation, or null if never toggled.</summary>
    public DateTimeOffset? LastChangedAt { get; private set; }

    // Reserved for EF Core materialization only.
    private UserProfileAssignment()
    {
        ProfileId = null!;
        Scope = null!;
    }

    private UserProfileAssignment(
        UserProfileAssignmentId id,
        Guid userId,
        ProfileId profileId,
        AuthorizationScope scope,
        DateTimeOffset assignedAt)
        : base(id)
    {
        UserId = userId;
        ProfileId = profileId;
        Scope = scope;
        AssignedAt = assignedAt;
        IsActive = true;
        LastChangedAt = null;
    }

    /// <summary>
    /// Creates a new UserProfileAssignment. This is the only way an
    /// assignment comes into existence, and it is always created active
    /// — callers (see <c>AssignUserToProfileCommandHandler</c>) are
    /// responsible for deactivating the user's previously active
    /// assignment, if any, in the same operation.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="profileId">The identifier of the profile.</param>
    /// <param name="scope">The authorization scope.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <returns>A result containing the new assignment, or a validation error.</returns>
    public static Result<UserProfileAssignment> Create(
        Guid userId,
        ProfileId profileId,
        AuthorizationScope scope,
        IDateTimeProvider dateTimeProvider)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<UserProfileAssignment>(ProfileErrors.UserIdRequired());
        }

        if (profileId is null)
        {
            return Result.Failure<UserProfileAssignment>(ProfileErrors.ProfileIdRequired());
        }

        if (scope is null)
        {
            return Result.Failure<UserProfileAssignment>(ProfileErrors.ScopeRequired());
        }

        var assignment = new UserProfileAssignment(
            UserProfileAssignmentId.New(),
            userId,
            profileId,
            scope,
            dateTimeProvider.UtcNow);

        assignment.RaiseDomainEvent(
            new UserAssignedToProfile(
                assignment.Id,
                userId,
                profileId,
                scope,
                assignment.AssignedAt));

        return assignment;
    }

    /// <summary>
    /// Deactivates this assignment. Idempotent: deactivating an already
    /// inactive assignment is a harmless no-op (chat, 2026-08-25 —
    /// revised), since the whole point of this model is that assignments
    /// toggle freely.
    /// </summary>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public void Deactivate(IDateTimeProvider dateTimeProvider)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        LastChangedAt = dateTimeProvider.UtcNow;

        RaiseDomainEvent(
            new UserProfileAssignmentDeactivated(Id, UserId, ProfileId, Scope, LastChangedAt.Value));
    }

    /// <summary>
    /// Activates this assignment. Idempotent for the same reason as
    /// <see cref="Deactivate"/>. Callers (see
    /// <c>ActivateUserProfileAssignmentCommandHandler</c>) are
    /// responsible for deactivating whichever other assignment is
    /// currently active for the same user, in the same operation, so
    /// the "at most one active Profile" invariant holds afterward.
    /// </summary>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public void Activate(IDateTimeProvider dateTimeProvider)
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        LastChangedAt = dateTimeProvider.UtcNow;

        RaiseDomainEvent(
            new UserProfileAssignmentActivated(Id, UserId, ProfileId, Scope, LastChangedAt.Value));
    }
}