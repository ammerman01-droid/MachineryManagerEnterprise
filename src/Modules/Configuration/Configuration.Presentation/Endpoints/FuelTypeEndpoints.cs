using MachineryManager.Configuration.Application.Features.FuelTypes.Commands.RegisterFuelType;
using MachineryManager.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypesByHolding;
using MachineryManager.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Configuration.Presentation.Endpoints;

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

    /// <summary>Handles <c>GET /api/v1/fuel-types?holdingId=...</c>.</summary>
    /// <param name="holdingId">The Holding whose fuel type list should be returned (required query parameter).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetFuelTypesByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the list of fuel types on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetFuelTypesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFuelTypesByHoldingQuery(holdingId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}