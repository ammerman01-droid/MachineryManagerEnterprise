using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace MachineryManager.Identity.Presentation.Endpoints;

/// <summary>
/// Development-only diagnostic endpoint that returns the current
/// user's stored OpenIddict tokens as JSON, for manual API testing
/// with tools like Postman/curl (chat, 2026-08-20).
/// </summary>
/// <remarks>
/// Registered ONLY when the hosting environment is Development
/// (checked once at startup, not per-request) — the route does not
/// exist at all outside Development, not merely hidden behind a
/// runtime check.
/// </remarks>
public static class DevTokenEndpoints
{
    /// <summary>Registers the Development-only token inspection endpoint, if the environment is Development.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <param name="environment">The hosting environment.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapIdentityDevTokenEndpoints(
        this IEndpointRouteBuilder endpoints,
        IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            return endpoints;
        }

        Func<HttpContext, Task<IResult>> handler = HandleAsync;

        endpoints.MapGet("/identity/dev/token", handler)
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                .RequireAuthenticatedUser());

        return endpoints;
    }

    private static async Task<IResult> HandleAsync(HttpContext httpContext)
    {
        var accessToken = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "access_token");
        var refreshToken = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "refresh_token");
        var expiresAt = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "expires_at");

        if (accessToken is null)
        {
            return Results.NotFound(new
            {
                message = "No access token stored on the current session. Sign in via /identity/connect first.",
            });
        }

        return Results.Ok(new
        {
            access_token = accessToken,
            refresh_token = refreshToken,
            expires_at = expiresAt,
            user_name = httpContext.User.Identity?.Name,
        });
    }
}