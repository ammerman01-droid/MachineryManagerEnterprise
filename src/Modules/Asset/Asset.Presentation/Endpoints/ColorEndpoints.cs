using MachineryManager.Asset.Application.Features.Colors.Commands.RegisterColor;
using MachineryManager.Asset.Application.Features.Colors.Queries.GetColorsByOrganization;
using MachineryManager.Asset.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Asset.Presentation.Endpoints;

/// <summary>Maps the Asset module's Color REST endpoints: base path <c>/api/v1/colors</c>.</summary>
public static class ColorEndpoints
{
    /// <summary>Registers the Color endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapColorEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/colors")
            .WithTags("Colors")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterColorAsync)
            .WithName("RegisterColor")
            .WithSummary("Registers a new Color option within an Organization.");

        group.MapGet("/", GetColorsByOrganizationAsync)
            .WithName("GetColorsByOrganization")
            .WithSummary("Retrieves every Color registered for an Organization.");

        return endpoints;
    }

    private static async Task<IResult> RegisterColorAsync(
        RegisterColorRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterColorCommand(request.OrganizationId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/colors/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetColorsByOrganizationAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetColorsByOrganizationQuery(organizationId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }
}