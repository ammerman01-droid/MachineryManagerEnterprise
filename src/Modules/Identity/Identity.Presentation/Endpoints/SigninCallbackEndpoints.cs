using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Client.AspNetCore;

namespace MachineryManager.Identity.Presentation.Endpoints;

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
        Func<HttpContext, Task<IResult>> callbackHandler = HandleCallbackAsync;
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

    private static async Task<IResult> HandleCallbackAsync(HttpContext httpContext)
    {
        var result = await httpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

        if (!result.Succeeded || result.Principal is null)
        {
            return Results.Problem(
                title: "External sign-in failed.",
                detail: result.Failure?.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }

        var properties = result.Properties ?? new AuthenticationProperties();

        // OpenIddict.Client stores the issued tokens under its own
        // property keys, NOT under the plain "access_token"/"refresh_token"
        // names that HttpContext.GetTokenAsync expects by convention.
        // Re-store them explicitly so later GetTokenAsync calls (e.g.
        // /identity/dev/token) can find them.
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

        await httpContext.SignInAsync(IdentityConstants.ApplicationScheme, result.Principal, properties);

        var returnUrl = properties.RedirectUri;

        return Results.Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
    }
}