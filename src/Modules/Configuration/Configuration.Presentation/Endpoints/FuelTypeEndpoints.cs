using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.ActivateFuelType;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.DeactivateFuelType;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.RegisterFuelType;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.UpdateFuelType;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypeById;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypesByHolding;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Fuel Type REST endpoints: base path <c>/api/v1/fuel-types</c>.</summary>
public static class FuelTypeEndpoints
{
    /// <summary>Registers the Fuel Type endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapFuelTypeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/fuel-types")
            .WithTags("FuelTypes")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterFuelTypeAsync)
            .WithName("RegisterFuelType")
            .WithSummary("Registers a new Fuel Type within a Holding.");

        group.MapGet("/", GetFuelTypesByHoldingAsync)
            .WithName("GetFuelTypesByHolding")
            .WithSummary("Retrieves every Fuel Type registered for a Holding.");

        group.MapGet("/{id:guid}", GetFuelTypeByIdAsync)
            .WithName("GetFuelTypeById")
            .WithSummary("Retrieves a single Fuel Type by its identifier.");

        group.MapPut("/{id:guid}", UpdateFuelTypeAsync)
            .WithName("UpdateFuelType")
            .WithSummary("Updates an existing Fuel Type's name, price, and kind.");

        group.MapPatch("/{id:guid}/deactivate", DeactivateFuelTypeAsync)
            .WithName("DeactivateFuelType")
            .WithSummary("Deactivates (soft-deletes) a Fuel Type.");

        group.MapPatch("/{id:guid}/activate", ActivateFuelTypeAsync)
            .WithName("ActivateFuelType")
            .WithSummary("Reactivates a previously deactivated Fuel Type.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/fuel-types</c>.</summary>
    /// <param name="request">The registration payload.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterFuelTypeCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>201 Created</c> with the new fuel type's id on success; otherwise a standard error body.</returns>
    private static async Task<IResult> RegisterFuelTypeAsync(
        RegisterFuelTypeRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterFuelTypeCommand(request.HoldingId, request.Name, request.Price, request.Kind), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/fuel-types/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/fuel-types?holdingId=...&amp;includeInactive=...</c>.</summary>
    /// <param name="holdingId">The Holding whose fuel type list should be returned (required query parameter).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetFuelTypesByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <param name="includeInactive">
    /// When <see langword="true"/>, deactivated fuel types are included
    /// (used by the admin management page). Defaults to <see langword="false"/>.
    /// </param>
    /// <returns><c>200 OK</c> with the list of fuel types on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetFuelTypesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        bool includeInactive = false)
    {
        var result = await sender.Send(new GetFuelTypesByHoldingQuery(holdingId, includeInactive), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/fuel-types/{id}</c>.</summary>
    /// <param name="id">The identifier of the Fuel Type to retrieve.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetFuelTypeByIdQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the Fuel Type on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetFuelTypeByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFuelTypeByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/fuel-types/{id}</c>.</summary>
    /// <param name="id">The identifier of the Fuel Type to update.</param>
    /// <param name="request">The update payload (Name, Price, Kind).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="UpdateFuelTypeCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> UpdateFuelTypeAsync(
        Guid id,
        UpdateFuelTypeRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateFuelTypeCommand(id, request.Name, request.Price, request.Kind), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PATCH /api/v1/fuel-types/{id}/deactivate</c>.</summary>
    /// <param name="id">The identifier of the Fuel Type to deactivate.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="DeactivateFuelTypeCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> DeactivateFuelTypeAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateFuelTypeCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PATCH /api/v1/fuel-types/{id}/activate</c>.</summary>
    /// <param name="id">The identifier of the Fuel Type to reactivate.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="ActivateFuelTypeCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> ActivateFuelTypeAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateFuelTypeCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }
}
