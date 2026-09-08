using MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;
using MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Queries.GetUnitsOfMeasurementByHolding;
using MachineryManager.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Unit of Measurement REST endpoints: base path <c>/api/v1/units-of-measurement</c>.</summary>
public static class UnitOfMeasurementEndpoints
{
    /// <summary>Registers the Unit of Measurement endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapUnitOfMeasurementEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/units-of-measurement")
            .WithTags("UnitsOfMeasurement")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterUnitOfMeasurementAsync)
            .WithName("RegisterUnitOfMeasurement")
            .WithSummary("Registers a new Unit of Measurement within a Holding.");

        group.MapGet("/", GetUnitsOfMeasurementByHoldingAsync)
            .WithName("GetUnitsOfMeasurementByHolding")
            .WithSummary("Retrieves every Unit of Measurement registered for a Holding.");

        return endpoints;
    }

/// <summary>Handles <c>POST /api/v1/units-of-measurement</c>.</summary>
    /// <param name="request">The registration payload (HoldingId, Name, CategoryId).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterUnitOfMeasurementCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns>
    /// <c>201 Created</c> with the new unit's id on success; a
    /// <c>404</c> if <c>CategoryId</c> doesn't exist, a <c>409</c> if it
    /// belongs to a different Holding, or another standard error body.
    /// </returns>
    private static async Task<IResult> RegisterUnitOfMeasurementAsync(
        RegisterUnitOfMeasurementRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterUnitOfMeasurementCommand(request.HoldingId, request.Name, request.Kind),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/units-of-measurement/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/units-of-measurement?holdingId=...</c>.</summary>
    /// <param name="holdingId">The Holding whose unit list should be returned (required query parameter).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetUnitsOfMeasurementByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the list of units (each including its category's display name) on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetUnitsOfMeasurementByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUnitsOfMeasurementByHoldingQuery(holdingId), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}