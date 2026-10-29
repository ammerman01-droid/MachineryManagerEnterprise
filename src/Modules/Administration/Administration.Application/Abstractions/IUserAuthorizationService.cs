using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>
/// Decides who may see and manage users and user-profile assignments. Rules (Section 5.8):
/// Platform and Holding users may manage users of their own level and below; Organization
/// users only strictly below (Project level); Project users never. Every action also needs the
/// matching <c>User.*</c> permission on the target scope, and nobody may grant a profile
/// containing permissions they do not hold.
/// </summary>
public interface IUserAuthorizationService
{
    /// <summary>Resolves the current caller's own scope; fails when unauthenticated or unassigned.</summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The caller's scope, or an error.</returns>
    Task<Result<ActorScope>> GetActorAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Authorizes creating a user, or assigning/changing an assignment, at <paramref name="target"/>:
    /// level rule, containment in the caller's scope, and the <c>User.{action}</c> permission.
    /// </summary>
    /// <param name="actor">The caller.</param>
    /// <param name="target">The target scope chain.</param>
    /// <param name="action">The User action: "Create" or "Edit".</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Success, or a forbidden error.</returns>
    Task<Result> AuthorizeScopeAsync(
        ActorScope actor,
        ScopeChain target,
        string action,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a user for a mutating action. NotFound when the user does not exist or lies outside the
    /// caller's scope (so other tenants' users are never revealed); then the level rule and the
    /// <c>User.{action}</c> permission on the user's home.
    /// </summary>
    /// <param name="actor">The caller.</param>
    /// <param name="userId">The user.</param>
    /// <param name="action">The User action: "Edit".</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The user with the resolved home chain, or an error.</returns>
    Task<Result<ManagedUser>> LoadUserForManagementAsync(
        ActorScope actor,
        Guid userId,
        string action,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a user for reading: NotFound when outside the caller's scope, otherwise requires
    /// <c>User.View</c> on the user's home.
    /// </summary>
    /// <param name="actor">The caller.</param>
    /// <param name="userId">The user.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The user with the resolved home chain, or an error.</returns>
    Task<Result<ManagedUser>> LoadUserForViewAsync(
        ActorScope actor,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>Determines which users the current caller may list.</summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The visibility rule, or a forbidden error.</returns>
    Task<Result<UserVisibility>> ResolveUserVisibilityAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks that <paramref name="profile"/> may be given to a user at <paramref name="target"/>:
    /// it is active, owned by the target's Holding, and contains only permissions the caller holds.
    /// </summary>
    /// <param name="actor">The caller.</param>
    /// <param name="target">The target scope chain.</param>
    /// <param name="profile">The profile to assign.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Success, or an error.</returns>
    Task<Result> AuthorizeProfileForAssignmentAsync(
        ActorScope actor,
        ScopeChain target,
        global::Administration.Domain.Profile profile,
        CancellationToken cancellationToken = default);
}
