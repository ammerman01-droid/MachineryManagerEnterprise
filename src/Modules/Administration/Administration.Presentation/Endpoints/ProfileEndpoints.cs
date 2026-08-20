using MachineryManager.Administration.Application.Features.Profiles.Commands.CreateProfile;
using MachineryManager.Administration.Application.Features.Profiles.Queries.GetProfileById;
using MachineryManager.Administration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

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
            .WithSummary("Creates a new permission Profile.");

        group.MapGet("/{profileId:guid}", GetProfileByIdAsync)
            .WithName("GetProfileById")
            .WithSummary("Retrieves a single Profile by its identifier.");

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
}