using Administration.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Administration.Domain;

/// <summary>
/// Represents the assignment of a <see cref="Profile"/> to a User at a
/// specific <see cref="AuthorizationScope"/> (Section 5.8). This is a
/// first-class aggregate because its lifecycle is independent from both
/// the User (Identity module) and the Profile.
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
    /// Gets whether this assignment has been revoked. A revoked assignment
    /// is excluded from authorization checks (BR-017, Access revocation on
    /// reassignment) but is never physically deleted — Audit Requirements
    /// (Section 5.8) require immutable authorization records.
    /// </summary>
    public bool IsRevoked { get; private set; }

    /// <summary>Gets the UTC timestamp when the assignment was revoked, or null if still active.</summary>
    public DateTimeOffset? RevokedAt { get; private set; }

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
        IsRevoked = false;
        RevokedAt = null;
    }

    /// <summary>
    /// Creates a new UserProfileAssignment. This is the only way an assignment comes into existence.
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
    /// Revokes this assignment, immediately excluding it from authorization
    /// checks (BR-017). The record itself is retained (soft revocation) to
    /// satisfy the immutable-audit-trail requirement in Section 5.8.
    /// </summary>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <returns>A result indicating success, or a business error if already revoked.</returns>
    public Result Revoke(IDateTimeProvider dateTimeProvider)
    {
        if (IsRevoked)
        {
            return Result.Failure(ProfileErrors.AssignmentAlreadyRevoked());
        }

        IsRevoked = true;
        RevokedAt = dateTimeProvider.UtcNow;

        RaiseDomainEvent(
            new UserProfileAssignmentRevoked(Id, UserId, ProfileId, Scope, RevokedAt.Value));

        return Result.Success();
    }
}