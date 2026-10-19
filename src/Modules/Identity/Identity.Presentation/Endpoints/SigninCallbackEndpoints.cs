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
                RedirectUri = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl,
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

        // The principal built by OpenIddict.Client from the identity token has no
        // ASP.NET Core Identity security stamp (it is deliberately never put in any
        // token). Using it as the cookie principal made SecurityStampValidator reject
        // the cookie and sign the user out. Rebuild the cookie principal from the
        // user store instead, so it carries the stamp, roles and tenant-scope claims.
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

        // Let the cookie handler stamp a fresh issue time and derive the expiry from
        // the scheme's ExpireTimeSpan; neither may inherit the access token's lifetime.
        properties.IssuedUtc = null;
        properties.ExpiresUtc = null;

        // OpenIddict.Client stores the issued tokens under its own
        // property keys, NOT under the plain "access_token"/"refresh_token"
        // names that HttpContext.GetTokenAsync expects by convention.
        // Re-store them explicitly so later GetTokenAsync calls can find them.
        var tokens = new List<AuthenticationToken>();

        var accessToken = properties.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken);
        if (!string.IsNullOrEmpty(accessToken))
        {
            tokens.Add(new AuthenticationToken { Name = "access_token", Value = accessToken });
        }

        var refreshToken = properties.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken);
        if (!string.IsNullOrEmpty(refreshToken))
        {
            tokens.Add(new AuthenticationToken { Name = "refresh_token", Value = refreshToken });
        }

        var identityToken = properties.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken);
        if (!string.IsNullOrEmpty(identityToken))
        {
            tokens.Add(new AuthenticationToken { Name = "id_token", Value = identityToken });
        }

        properties.StoreTokens(tokens);
        await httpContext.SignInAsync(IdentityConstants.ApplicationScheme, cookiePrincipal, properties);

        var returnUrl = properties.RedirectUri;
        return Results.Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
    }
}