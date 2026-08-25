namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Evaluates, at request time, whether a User has a given Permission
/// over a specific resource — per the SuperUser / partial-scope
/// Administrator model (05-application, Section 5.8). Deliberately
/// NOT cached or claims-based, so revocation and reassignment take
/// effect immediately (BR-017).
/// </summary>
public interface IPermissionEvaluator
{
    /// <summary>Determines whether the given user holds the given permission over the given resource scope.</summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="permission">The permission string (e.g. "Organization.Manage").</param>
    /// <param name="resourceScope">The full scope chain of the resource being accessed.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><c>true</c> if the user is authorized; otherwise <c>false</c>.</returns>
    Task<bool> HasPermissionAsync(
        Guid userId,
        string permission,
        ResourceScope resourceScope,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Resolves the full set of scopes the user holds the given permission
    /// over, for filtering list/search results (05-application, Section 5.8,
    /// Phase 3 — Scope-based Filtering).
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="permission">The permission string (e.g. "Organization.View").</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The set of scopes the user is authorized within for this permission.</returns>
    Task<AuthorizedScopeSet> GetAuthorizedScopesAsync(
        Guid userId,
        string permission,
        CancellationToken cancellationToken = default);
}