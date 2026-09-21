using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManagerEnterprise.Identity.Presentation.Tests.Infrastructure;

/// <summary>Options bag required by <see cref="AuthenticationHandler{TOptions}"/>; nothing to configure here.</summary>
public sealed class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
}

/// <summary>
/// Stands in for real OpenIddict token validation
/// (<c>OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme</c>)
/// in these endpoint slice tests, so <c>UserEndpoints</c>' own
/// authorization logic (<c>RequireAuthenticatedUser</c>,
/// <c>RequireClaim(Claims.Role, "System Administrator")</c>) can be
/// exercised without standing up a full OpenIddict Authorization
/// Server and issuing real tokens — that protocol-level concern is out
/// of scope for this slice (see the completion report).
/// </summary>
/// <remarks>
/// Each request opts into an identity via two headers, set explicitly
/// per test:
/// <list type="bullet">
/// <item><see cref="AuthenticatedHeader"/> = "true" to authenticate at all (its absence yields 401, exactly like a missing/invalid Bearer token would).</item>
/// <item><see cref="RoleHeader"/> = a role name, to also carry that role claim (used by the endpoints requiring "System Administrator").</item>
/// </list>
/// </remarks>
public sealed class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{
    /// <summary>Header a test sets to "true" to simulate an authenticated caller.</summary>
    public const string AuthenticatedHeader = "X-Test-Authenticated";

    /// <summary>Header a test sets to carry a role claim (e.g. "System Administrator") on the simulated principal.</summary>
    public const string RoleHeader = "X-Test-Role";

    public TestAuthHandler(
        IOptionsMonitor<TestAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <inheritdoc />
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(AuthenticatedHeader, out var authenticated) ||
            !string.Equals(authenticated, "true", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>
        {
            new(Claims.Subject, "00000000-0000-0000-0000-000000000001"),
        };

        if (Request.Headers.TryGetValue(RoleHeader, out var role) && !string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(Claims.Role, role.ToString()));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
