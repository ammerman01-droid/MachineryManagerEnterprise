using System.Security.Claims;
using MachineryManager.Identity.Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManager.Identity.Presentation.Endpoints;

/// <summary>
/// Maps the OpenIddict protocol endpoints (Authorization Code + PKCE
/// for interactive users, Client Credentials for Service-to-Service),
/// per the decisions made in chat (2026-08-18).
/// </summary>
public static class ConnectEndpoints
{
    /// <summary>Registers the Identity module's OpenIddict protocol endpoints.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapIdentityConnectEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapMethods("/connect/authorize", ["GET", "POST"], AuthorizeAsync);
        endpoints.MapPost("/connect/token", ExchangeAsync);
        endpoints.MapMethods("/connect/logout", ["GET", "POST"], LogoutAsync);
        endpoints.MapMethods("/connect/userinfo", ["GET", "POST"], UserInfoAsync);

        return endpoints;
    }

    private static async Task<IResult> AuthorizeAsync(
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        var request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var authenticateResult = await httpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        if (!authenticateResult.Succeeded || authenticateResult.Principal is null)
        {
            var returnUrl = Uri.EscapeDataString(
                httpContext.Request.PathBase + httpContext.Request.Path + httpContext.Request.QueryString);

            return Results.Redirect($"/identity/login?ReturnUrl={returnUrl}");
        }

        var user = await userManager.GetUserAsync(authenticateResult.Principal)
            ?? throw new InvalidOperationException("The user details cannot be retrieved.");

        if (!await signInManager.CanSignInAsync(user))
        {
            return Results.Forbid(
                authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme],
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.AccessDenied,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in.",
                }));
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(request.GetScopes());

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetDestinations(claim, principal));
        }

        return Results.SignIn(principal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static async Task<IResult> ExchangeAsync(
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        var request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            var authenticateResult = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var user = authenticateResult.Principal is null
                ? null
                : await userManager.GetUserAsync(authenticateResult.Principal);

            if (user is null || !await signInManager.CanSignInAsync(user))
            {
                return Results.Forbid(
                    authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme],
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid.",
                    }));
            }

            var principal = await signInManager.CreateUserPrincipalAsync(user);
            principal.SetScopes(authenticateResult.Principal!.GetScopes());

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            return Results.SignIn(principal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsClientCredentialsGrantType())
        {
            // The principal represents the calling application itself,
            // not a human user — per the "Service-to-Service" decision
            // (chat, 2026-08-18).
            var identity = new ClaimsIdentity(
                authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                nameType: Claims.Name,
                roleType: Claims.Role);

            identity.SetClaim(Claims.Subject, request.ClientId ?? throw new InvalidOperationException("The client identifier cannot be retrieved."));
            identity.SetClaim(Claims.Name, request.ClientId);

            var principal = new ClaimsPrincipal(identity);
            principal.SetScopes(request.GetScopes());

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(Destinations.AccessToken);
            }

            return Results.SignIn(principal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return Results.BadRequest(new { error = Errors.UnsupportedGrantType });
    }

    private static async Task<IResult> LogoutAsync(SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();

        return Results.SignOut(
            authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme],
            properties: new AuthenticationProperties { RedirectUri = "/" });
    }

    private static async Task<IResult> UserInfoAsync(
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager)
    {
        var authenticateResult = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var user = authenticateResult.Principal is null
            ? null
            : await userManager.GetUserAsync(authenticateResult.Principal);

        if (user is null)
        {
            return Results.Challenge(authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]);
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            [Claims.Subject] = user.Id.ToString(),
        };

        if (authenticateResult.Principal!.HasScope(Scopes.Profile))
        {
            claims[Claims.PreferredUsername] = user.UserName ?? string.Empty;
        }

        return Results.Ok(claims);
    }

    /// <summary>
    /// Determines which token (access token, identity token, or both) a
    /// given claim should be embedded in, based on the granted scopes.
    /// Standard OpenIddict pattern — no undocumented business rule.
    /// </summary>
    private static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
    {
        switch (claim.Type)
        {
            case Claims.Name:
            case Claims.PreferredUsername:
                yield return Destinations.AccessToken;
                if (principal.HasScope(Scopes.Profile))
                {
                    yield return Destinations.IdentityToken;
                }

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;
                if (principal.HasScope(Scopes.Email))
                {
                    yield return Destinations.IdentityToken;
                }

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;
                if (principal.HasScope(Scopes.Roles))
                {
                    yield return Destinations.IdentityToken;
                }

                yield break;

            // Never expose the security stamp in issued tokens.
            case "AspNet.Identity.SecurityStamp":
                yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}