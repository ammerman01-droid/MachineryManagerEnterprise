using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Administration.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Administration.Domain.UserProfileAssignment"/> aggregate.</summary>
public interface IUserProfileAssignmentRepository : IRepository<global::Administration.Domain.UserProfileAssignment, global::Administration.Domain.UserProfileAssignmentId>
{
    /// <summary>Retrieves all assignments for a given user.</summary>
    Task<IReadOnlyList<global::Administration.Domain.UserProfileAssignment>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the user's currently active (non-revoked) assignments.
    /// Per the "one active Profile per user" rule (chat, 2026-08-25) this
    /// is expected to contain at most one entry going forward, but is
    /// returned as a list defensively in case older data predates the
    /// rule.
    /// </summary>
    Task<IReadOnlyList<global::Administration.Domain.UserProfileAssignment>> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns whether the given Profile has at least one active
    /// (non-revoked) assignment to any user — used to block Profile
    /// deletion while it is still in use (chat, 2026-08-25).
    /// </summary>
    Task<bool> HasActiveAssignmentsForProfileAsync(
        global::Administration.Domain.ProfileId profileId,
        CancellationToken cancellationToken = default);
}
