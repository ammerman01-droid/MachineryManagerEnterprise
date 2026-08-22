using MachineryManager.Administration.Application.Features.Profiles.Commands.ActivateProfile;
using MachineryManager.Administration.Application.Features.Profiles.Commands.CreateProfile;
using MachineryManager.Administration.Application.Features.Profiles.Commands.DeactivateProfile;
using MachineryManager.Administration.Application.Features.Profiles.Commands.UpdateProfile;
using MachineryManager.Administration.Application.Features.Profiles.Queries.GetProfileById;
using MachineryManager.Administration.Application.Features.Profiles.Queries.SearchProfiles;
using MachineryManager.Administration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManager.Administration.Presentation.Endpoints;

/// <summary>Maps the Profile module's REST endpoints.</summary>
public static class ProfileEndpoints
{
    /// <summary>Registers the Profile module's endpoints.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapProfileEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/profiles")
            .WithTags("Profiles")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", CreateProfileAsync)
            .WithName("CreateProfile")
            .WithSummary("Creates a new permission Profile.")
            // Bootstrap-phase restriction (chat, 2026-08-20): only a
            // Platform SuperUser (System Administrator) may create
            // Profiles. Full scope-aware SuperUser resolution (Holding/
            // Organization/Project) is a documented follow-up — this is
            // NOT the final permission model, just closing the
            // immediate privilege-escalation gap.
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapPut("/{profileId:guid}", UpdateProfileAsync)
            .WithName("UpdateProfile")
            .WithSummary("Updates a Profile's name and permissions.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapPost("/{profileId:guid}/deactivate", DeactivateProfileAsync)
            .WithName("DeactivateProfile")
            .WithSummary("Deactivates a Profile.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapPost("/{profileId:guid}/activate", ActivateProfileAsync)
            .WithName("ActivateProfile")
            .WithSummary("Activates a Profile.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapGet("/{profileId:guid}", GetProfileByIdAsync)
            .WithName("GetProfileById")
            .WithSummary("Retrieves a single Profile by its identifier.");

        group.MapGet("/", SearchProfilesAsync)
            .WithName("SearchProfiles")
            .WithSummary("Searches Profiles with optional text filtering and pagination.");

        return endpoints;
    }

    private static async Task<IResult> CreateProfileAsync(
        CreateProfileRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateProfileCommand(request.Name, request.Permissions),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/profiles/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> UpdateProfileAsync(
        Guid profileId,
        UpdateProfileRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateProfileCommand(profileId, request.Name, request.Permissions),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DeactivateProfileAsync(
        Guid profileId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new DeactivateProfileCommand(profileId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ActivateProfileAsync(
        Guid profileId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ActivateProfileCommand(profileId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetProfileByIdAsync(
        Guid profileId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProfileByIdQuery(profileId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchProfilesAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchProfilesQuery(search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }
}