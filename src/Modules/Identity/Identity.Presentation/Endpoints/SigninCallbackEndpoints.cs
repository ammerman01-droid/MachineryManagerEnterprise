using System.Globalization;
using System.Security.Claims;
using MachineryManagerEnterprise.Identity.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;

namespace MachineryManagerEnterprise.Identity.Presentation.Endpoints;

/// <summary>
/// Handles the redirection back from this application's own OpenIddict
/// Authorization Server after a successful Authorization Code + PKCE
/// exchange (initiated by <see cref="ConnectEndpoints"/>'s /connect/authorize
/// passthrough — this endpoint completes the client side).
/// </summary>
public static class SigninCallbackEndpoints
{
    /// <summary>OpenIddict.Client's property key for the back-channel access token's expiry.</summary>
    private const string AccessTokenExpirationKey = "backchannel_access_token_expiration_date";

    /// <summary>Conservative expiry used when the issuer did not report one (below the server's token lifetime).</summary>
    private static readonly TimeSpan FallbackAccessTokenLifetime = TimeSpan.FromMinutes(10);

    /// <summary>Registers the OpenIddict Client's redirection endpoint handler.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapIdentitySigninCallbackEndpoints(this IEndpointRouteBuilder endpoints)
    {
        Func<HttpContext, UserManager<ApplicationUser>, SignInManager<ApplicationUser>, Task<IResult>> callbackHandler =
            HandleCallbackAsync;
        endpoints.MapGet("/signin-oidc", callbackHandler);

        // Starts the flow: triggers OpenIddict.Client to generate a real
        // PKCE code_verifier/code_challenge, store the correlation state,
        // and redirect to /connect/authorize.
        endpoints.MapGet("/identity/connect", (string? returnUrl) =>
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = SanitizeReturnUrl(returnUrl),
            };

            return Results.Challenge(properties, [OpenIddictClientAspNetCoreDefaults.AuthenticationScheme]);
        });

        return endpoints;
    }

    private static async Task<IResult> HandleCallbackAsync(
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        var result = await httpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

        if (!result.Succeeded || result.Principal is null)
        {
            return Results.Problem(
                title: "External sign-in failed.",
                detail: result.Failure?.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }

        // The cookie principal must come from the user store (it carries the security
        // stamp, roles and tenant-scope claims), not from the identity token.
        var subject = result.Principal.FindFirstValue(OpenIddictConstants.Claims.Subject)
            ?? result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = subject is null ? null : await userManager.FindByIdAsync(subject);

        if (user is null || !await signInManager.CanSignInAsync(user))
        {
            return Results.Problem(
                title: "Sign-in is not allowed for this user.",
                statusCode: StatusCodes.Status403Forbidden);
        }

        var cookiePrincipal = await signInManager.CreateUserPrincipalAsync(user);

        // کوکی معتبری که PasswordSignInAsync قبلاً با انتخاب واقعی
        // کاربر برای RememberMe ساخته را می‌خوانیم، تا این SignIn دوم
        // آن انتخاب را از بین نبرد.
        var existingAuth = await httpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        var properties = result.Properties ?? new AuthenticationProperties();

        properties.IsPersistent = existingAuth.Properties?.IsPersistent ?? false;

        // Let the cookie handler stamp fresh issue/expiry times from the scheme's
        // ExpireTimeSpan; neither may inherit the access token's lifetime.
        properties.IssuedUtc = null;
        properties.ExpiresUtc = null;

        // OpenIddict.Client keeps tokens under its own property keys; re-store them
        // under the conventional names that HttpContext.GetTokenAsync expects. The id
        // token is deliberately NOT stored (nothing uses it; it only inflated the cookie).
        var tokens = new List<AuthenticationToken>();

        var accessToken = properties.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken);
        if (!string.IsNullOrEmpty(accessToken))
        {
            tokens.Add(new AuthenticationToken { Name = "access_token", Value = accessToken });

            var expiresAt = ParseExpiry(properties.GetTokenValue(AccessTokenExpirationKey))
                ?? DateTimeOffset.UtcNow.Add(FallbackAccessTokenLifetime);
            tokens.Add(new AuthenticationToken
            {
                Name = "expires_at",
                Value = expiresAt.ToString("o", CultureInfo.InvariantCulture),
            });
        }

        var refreshToken = properties.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken);
        if (!string.IsNullOrEmpty(refreshToken))
        {
            tokens.Add(new AuthenticationToken { Name = "refresh_token", Value = refreshToken });
        }

        properties.StoreTokens(tokens);
        await httpContext.SignInAsync(IdentityConstants.ApplicationScheme, cookiePrincipal, properties);

        return Results.Redirect(SanitizeReturnUrl(properties.RedirectUri));
    }

    /// <summary>
    /// Accepts only application-local, path-absolute URLs ("/..."); anything else
    /// (absolute URLs, protocol-relative "//host", "/\host", control characters)
    /// falls back to "/", preventing open redirects after sign-in.
    /// </summary>
    private static string SanitizeReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl) || returnUrl[0] != '/')
        {
            return "/";
        }

        if (returnUrl.Length > 1 && (returnUrl[1] == '/' || returnUrl[1] == '\\'))
        {
            return "/";
        }

        return returnUrl.Any(char.IsControl) ? "/" : returnUrl;
    }

    private static DateTimeOffset? ParseExpiry(string? raw) =>
        DateTimeOffset.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var value)
            ? value
            : null;
}