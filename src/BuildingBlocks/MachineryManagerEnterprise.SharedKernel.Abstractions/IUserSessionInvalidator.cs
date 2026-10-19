namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Ends every authenticated session of a user (cookies and OAuth tokens). Used when
/// the user's access rights change and on sign-out, so stale sessions cannot keep
/// acting with outdated rights.
/// </summary>
public interface IUserSessionInvalidator
{
    /// <summary>
    /// Rotates the user's security stamp (invalidating their cookies) and revokes
    /// all of their issued tokens and authorizations.
    /// </summary>
    /// <param name="userId">The user whose sessions must end.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task InvalidateAsync(Guid userId, CancellationToken cancellationToken = default);
}