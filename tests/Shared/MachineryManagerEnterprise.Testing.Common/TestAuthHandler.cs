using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MachineryManagerEnterprise.Testing.Common;

/// <summary>
/// Authentication handler for endpoint tests that stands in for OpenIddict
/// token validation. It never inspects the request at all — it just
/// mirrors whatever <see cref="FakeCurrentUserService"/> currently says,
/// so a test controls "who's calling" the same way whether application
/// code asks via <c>ICurrentUserService</c> or the ASP.NET Core
/// authorization middleware asks via <c>HttpContext.User</c>.
/// </summary>
/// <remarks>
/// Register this under the SAME scheme name the real endpoints require
/// (e.g. <c>OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme</c>)
/// so <c>RequireAuthorization(policy => policy.AddAuthenticationSchemes(...))</c>
/// resolves to this handler instead of a real OpenIddict validation handler,
/// which the test host never registers.
/// </remarks>
public sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FakeCurrentUserService _currentUser;

    /// <summary>Initializes a new instance of the <see cref="TestAuthHandler"/> class.</summary>
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        FakeCurrentUserService currentUser)
        : base(options, logger, encoder)
    {
        _currentUser = currentUser;
    }

    /// <inheritdoc />
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_currentUser.UserId is not { } userId)
        {
            // No principal at all — RequireAuthenticatedUser() will
            // correctly reject this as 401/403, exactly like a request
            // with no (or an invalid) bearer token in production.
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
