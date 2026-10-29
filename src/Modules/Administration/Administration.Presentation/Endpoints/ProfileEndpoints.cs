using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.ActivateProfile;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.CreateProfile;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.DeactivateProfile;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.DeleteProfile;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.UpdateProfile;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.GetProfileById;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.SearchProfiles;
using MachineryManagerEnterprise.Administration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Administration.Presentation.Endpoints;

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

        // Authorization is enforced by the command/query handlers (Profile.* permissions on
        // the profile's owning scope, tenant visibility, anti-escalation), not by an
        // Identity role: Platform users manage Platform-owned profiles, Holding users
        // manage their own Holding's profiles.
        group.MapPost("/", CreateProfileAsync)
            .WithName("CreateProfile")
            .WithSummary("Creates a new permission Profile owned by a Holding (or by the Platform).");

        group.MapPut("/{profileId:guid}", UpdateProfileAsync)
            .WithName("UpdateProfile")
            .WithSummary("Updates a Profile's name and permissions.");

        group.MapPost("/{profileId:guid}/deactivate", DeactivateProfileAsync)
            .WithName("DeactivateProfile")
            .WithSummary("Deactivates a Profile.");

        group.MapPost("/{profileId:guid}/activate", ActivateProfileAsync)
            .WithName("ActivateProfile")
            .WithSummary("Activates a Profile.");

        group.MapDelete("/{profileId:guid}", DeleteProfileAsync)
            .WithName("DeleteProfile")
            .WithSummary("Permanently deletes a Profile. Blocked while it still has active user assignments.");

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
            new CreateProfileCommand(request.Name, request.Permissions, request.HoldingId),
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

    private static async Task<IResult> DeleteProfileAsync(
        Guid profileId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new DeleteProfileCommand(profileId),
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
