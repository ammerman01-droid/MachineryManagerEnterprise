using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>
/// Centralizes who may see and manage profiles. Profiles belong to a Holding (or to the
/// Platform); only Platform and that Holding's users may manage them, and nobody may
/// grant a permission they do not hold.
/// </summary>
public interface IProfileAuthorizationService
{
    /// <summary>Determines which profiles the current user is allowed to see.</summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The visibility rule, or a forbidden/unauthorized error.</returns>
    Task<Result<ProfileVisibility>> ResolveVisibilityAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Authorizes a mutating action on profiles owned by <paramref name="ownerHoldingId"/>.
    /// </summary>
    /// <param name="action">The Profile action: "Create", "Edit" or "Delete".</param>
    /// <param name="ownerHoldingId">The owning Holding, or <see langword="null"/> for Platform-owned profiles.</param>
    /// <param name="permissionsToGrant">The permissions the profile will contain, checked against the caller's own; or <see langword="null"/>.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Success, or a forbidden/unauthorized error.</returns>
    Task<Result> AuthorizeManageAsync(
        string action,
        Guid? ownerHoldingId,
        IReadOnlyCollection<string>? permissionsToGrant,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a profile for a mutating action: NotFound when it does not exist or is not
    /// visible to the caller (so other tenants' profiles are never revealed), then the
    /// authorization of <see cref="AuthorizeManageAsync"/>.
    /// </summary>
    /// <param name="profileId">The profile identifier.</param>
    /// <param name="action">The Profile action: "Edit" or "Delete".</param>
    /// <param name="permissionsToGrant">The permissions the profile will contain after the change, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The tracked profile, or an error.</returns>
    Task<Result<global::Administration.Domain.Profile>> GetForManagementAsync(
        Guid profileId,
        string action,
        IReadOnlyCollection<string>? permissionsToGrant,
        CancellationToken cancellationToken = default);
}

/// <summary>Describes which profiles a caller may see.</summary>
/// <param name="All">Whether every profile (Platform's and all Holdings') is visible.</param>
/// <param name="HoldingId">The only Holding whose profiles are visible when <paramref name="All"/> is false.</param>
public sealed record ProfileVisibility(bool All, Guid? HoldingId)
{
    /// <summary>Gets the visibility of a Platform-level caller: every profile.</summary>
    public static ProfileVisibility Everything { get; } = new(true, null);

    /// <summary>Returns whether a profile owned by <paramref name="ownerHoldingId"/> is visible.</summary>
    /// <param name="ownerHoldingId">The profile's owning Holding, or <see langword="null"/> for Platform-owned.</param>
    /// <returns><see langword="true"/> when visible.</returns>
    public bool CanSee(Guid? ownerHoldingId) =>
        All || (HoldingId is not null && ownerHoldingId == HoldingId);
}
