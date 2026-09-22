using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.ActivateColor;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.DeactivateColor;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.RegisterColor;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.UpdateColor;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Queries.GetColorById;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Queries.GetColorsByHolding;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

/// <summary>
/// Maps the Configuration module's Color REST endpoints per
/// 07-api-conventions.md, Section 8: base path <c>/api/v1/colors</c>.
/// </summary>
public static class ColorEndpoints
{
    /// <summary>Registers the Color endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
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
            .WithSummary("Registers a new Color within a Holding.");

        group.MapGet("/", GetColorsByHoldingAsync)
            .WithName("GetColorsByHolding")
            .WithSummary("Retrieves every Color registered for a Holding.");

        group.MapGet("/{id:guid}", GetColorByIdAsync)
            .WithName("GetColorById")
            .WithSummary("Retrieves a single Color by its identifier.");

        group.MapPut("/{id:guid}", UpdateColorAsync)
            .WithName("UpdateColor")
            .WithSummary("Updates an existing Color's display name.");

        group.MapPatch("/{id:guid}/deactivate", DeactivateColorAsync)
            .WithName("DeactivateColor")
            .WithSummary("Deactivates (soft-deletes) a Color.");

        group.MapPatch("/{id:guid}/activate", ActivateColorAsync)
            .WithName("ActivateColor")
            .WithSummary("Reactivates a previously deactivated Color.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/colors</c>.</summary>
    /// <param name="request">The registration payload (HoldingId, Name).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterColorCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns>
    /// <c>201 Created</c> with the new Color's id on success; otherwise a
    /// standard error body mapped from the failed <see cref="ResultExtensions"/>.
    /// </returns>
    private static async Task<IResult> RegisterColorAsync(
        RegisterColorRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterColorCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/colors/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/colors?holdingId=...&amp;includeInactive=...</c>.</summary>
    /// <param name="holdingId">The Holding whose Color catalog should be returned (required query parameter).</param>
    /// <param name="includeInactive">
    /// When <see langword="true"/>, deactivated colors are included
    /// (used by the admin management page). Defaults to <see langword="false"/>.
    /// </param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetColorsByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the list of colors on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetColorsByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        bool includeInactive = false)
    {
        var result = await sender.Send(new GetColorsByHoldingQuery(holdingId, includeInactive), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/colors/{id}</c>.</summary>
    /// <param name="id">The identifier of the Color to retrieve.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetColorByIdQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the Color on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetColorByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetColorByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/colors/{id}</c>.</summary>
    /// <param name="id">The identifier of the Color to update.</param>
    /// <param name="request">The update payload (Name).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="UpdateColorCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> UpdateColorAsync(
        Guid id,
        UpdateColorRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateColorCommand(id, request.Name), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PATCH /api/v1/colors/{id}/deactivate</c>.</summary>
    /// <param name="id">The identifier of the Color to deactivate.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="DeactivateColorCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> DeactivateColorAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateColorCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PATCH /api/v1/colors/{id}/activate</c>.</summary>
    /// <param name="id">The identifier of the Color to reactivate.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="ActivateColorCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> ActivateColorAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateColorCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }
}
