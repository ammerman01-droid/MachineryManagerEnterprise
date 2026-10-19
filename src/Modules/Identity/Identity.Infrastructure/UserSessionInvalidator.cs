using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace MachineryManagerEnterprise.Identity.Infrastructure;

/// <summary>
/// Identity-module implementation of <see cref="IUserSessionInvalidator"/>: rotates the
/// ASP.NET Core Identity security stamp and revokes the user's OpenIddict tokens and
/// authorizations (reference tokens are rejected as soon as their entry is revoked).
/// </summary>
internal sealed class UserSessionInvalidator : IUserSessionInvalidator
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly ILogger<UserSessionInvalidator> _logger;

    /// <summary>Initializes a new instance of the <see cref="UserSessionInvalidator"/> class.</summary>
    /// <param name="userManager">Used to rotate the security stamp.</param>
    /// <param name="tokenManager">Used to revoke issued tokens.</param>
    /// <param name="authorizationManager">Used to revoke issued authorizations.</param>
    /// <param name="logger">Diagnostic logger.</param>
    public UserSessionInvalidator(
        UserManager<ApplicationUser> userManager,
        IOpenIddictTokenManager tokenManager,
        IOpenIddictAuthorizationManager authorizationManager,
        ILogger<UserSessionInvalidator> logger)
    {
        _userManager = userManager;
        _tokenManager = tokenManager;
        _authorizationManager = authorizationManager;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task InvalidateAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var subject = userId.ToString();

        var user = await _userManager.FindByIdAsync(subject);
        if (user is not null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
        }

        // Materialize first: revoking while a streaming query is open would need MARS.
        var tokens = new List<object>();
        await foreach (var token in _tokenManager.FindBySubjectAsync(subject, cancellationToken))
        {
            tokens.Add(token);
        }

        var authorizations = new List<object>();
        await foreach (var authorization in _authorizationManager.FindBySubjectAsync(subject, cancellationToken))
        {
            authorizations.Add(authorization);
        }

        foreach (var token in tokens)
        {
            await _tokenManager.TryRevokeAsync(token, cancellationToken);
        }

        foreach (var authorization in authorizations)
        {
            await _authorizationManager.TryRevokeAsync(authorization, cancellationToken);
        }

        _logger.LogInformation(
            "Invalidated sessions of user {UserId}: {TokenCount} tokens, {AuthorizationCount} authorizations.",
            userId,
            tokens.Count,
            authorizations.Count);
    }
}