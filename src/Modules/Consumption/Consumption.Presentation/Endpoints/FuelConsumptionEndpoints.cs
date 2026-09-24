using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.DeleteFuelConsumption;
using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.EditFuelConsumption;
using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.RecordFuelConsumption;
using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.GetFuelConsumptionById;
using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.SearchFuelConsumptionsByAsset;
using MachineryManagerEnterprise.Consumption.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Consumption.Presentation.Endpoints;

/// <summary>Maps the Consumption module's Fuel Consumption REST endpoints: base path <c>/api/v1/fuel-consumptions</c>.</summary>
public static class FuelConsumptionEndpoints
{
    /// <summary>Registers the Fuel Consumption endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapFuelConsumptionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/fuel-consumptions")
            .WithTags("FuelConsumptions")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RecordFuelConsumptionAsync)
            .WithName("RecordFuelConsumption")
            .WithSummary("Records a new fuel-fill event for one fuel slot of an Asset.");

        group.MapGet("/{id:guid}", GetFuelConsumptionByIdAsync)
            .WithName("GetFuelConsumptionById")
            .WithSummary("Retrieves a single fuel-consumption record by its identifier.");

        group.MapGet("/", SearchFuelConsumptionsByAssetAsync)
            .WithName("SearchFuelConsumptionsByAsset")
            .WithSummary("Performs a paginated search over the fuel-consumption history of a single Asset.");

        group.MapPut("/{id:guid}", EditFuelConsumptionAsync)
            .WithName("EditFuelConsumption")
            .WithSummary("Edits the factual details of an existing fuel-consumption record.");

        group.MapDelete("/{id:guid}", DeleteFuelConsumptionAsync)
            .WithName("DeleteFuelConsumption")
            .WithSummary("Hard-deletes an existing fuel-consumption record.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/fuel-consumptions</c>.</summary>
    /// <param name="request">The record payload.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RecordFuelConsumptionCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>201 Created</c> with the new record's id on success; otherwise a standard error body.</returns>
    private static async Task<IResult> RecordFuelConsumptionAsync(
        RecordFuelConsumptionRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RecordFuelConsumptionCommand(
                request.AssetId,
                request.FuelSlot,
                request.FuelTypeId,
                request.Quantity,
                request.MeterReading,
                request.DeliveredByPersonnelId,
                request.ReceivedByPersonnelId,
                request.RecordedAtUtc,
                request.Notes),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/fuel-consumptions/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/fuel-consumptions/{id}</c>.</summary>
    private static async Task<IResult> GetFuelConsumptionByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFuelConsumptionByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/fuel-consumptions?assetId=...&amp;page=...&amp;pageSize=...</c>.</summary>
    private static async Task<IResult> SearchFuelConsumptionsByAssetAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 20)
    {
        var result = await sender.Send(new SearchFuelConsumptionsByAssetQuery(assetId, page, pageSize), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/fuel-consumptions/{id}</c>.</summary>
    private static async Task<IResult> EditFuelConsumptionAsync(
        Guid id,
        EditFuelConsumptionRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new EditFuelConsumptionCommand(
                id,
                request.FuelTypeId,
                request.Quantity,
                request.MeterReading,
                request.DeliveredByPersonnelId,
                request.ReceivedByPersonnelId,
                request.RecordedAtUtc,
                request.Notes),
            cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>DELETE /api/v1/fuel-consumptions/{id}</c>.</summary>
    private static async Task<IResult> DeleteFuelConsumptionAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteFuelConsumptionCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }
}
