using MachineryManager.Asset.Application.Features.EngineModels.Commands.RegisterEngineModel;
using MachineryManager.Asset.Application.Features.EngineModels.Commands.RenameEngineModel;
using MachineryManager.Asset.Application.Features.EngineModels.Commands.UpdateEngineModelSpecifications;
using MachineryManager.Asset.Application.Features.EngineModels.Queries.GetEngineModelById;
using MachineryManager.Asset.Application.Features.EngineModels.Queries.SearchEngineModels;
using MachineryManager.Asset.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Asset.Presentation.Endpoints;

/// <summary>
/// Maps the Asset module's Engine Model REST endpoints per 07-api
/// conventions (Section 8): base path <c>/api/v1/engine-models</c>.
/// </summary>
public static class EngineModelEndpoints
{
    /// <summary>Registers the Engine Model endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapEngineModelEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/engine-models")
            .WithTags("EngineModels")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterEngineModelAsync)
            .WithName("RegisterEngineModel")
            .WithSummary("Registers a new Engine Model within a Holding.");

        group.MapPut("/{engineModelId:guid}", RenameEngineModelAsync)
            .WithName("RenameEngineModel")
            .WithSummary("Renames an existing Engine Model.");

        group.MapPut("/{engineModelId:guid}/specifications", UpdateEngineModelSpecificationsAsync)
            .WithName("UpdateEngineModelSpecifications")
            .WithSummary("Updates the technical specifications of an existing Engine Model.");

        group.MapGet("/{engineModelId:guid}", GetEngineModelByIdAsync)
            .WithName("GetEngineModelById")
            .WithSummary("Retrieves a single Engine Model by its identifier.");

        group.MapGet("/", SearchEngineModelsAsync)
            .WithName("SearchEngineModels")
            .WithSummary("Searches Engine Models within a Holding, with optional text filtering and pagination.");

        return endpoints;
    }

    private static async Task<IResult> RegisterEngineModelAsync(
        RegisterEngineModelRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterEngineModelCommand(
                request.HoldingId,
                request.Name,
                request.CompanyId,
                request.CylinderCount,
                request.EngineDisplacementValue,
                request.EngineDisplacementUnitOfMeasurementId,
                request.EnginePowerValue,
                request.EnginePowerUnitOfMeasurementId,
                request.WeightValue,
                request.WeightUnitOfMeasurementId),
                cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/engine-models/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RenameEngineModelAsync(
        Guid engineModelId,
        RenameEngineModelRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RenameEngineModelCommand(engineModelId, request.Name),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> UpdateEngineModelSpecificationsAsync(
        Guid engineModelId,
        UpdateEngineModelSpecificationsRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateEngineModelSpecificationsCommand(
                engineModelId,
                request.CompanyId,
                request.CylinderCount,
                request.EngineDisplacementValue,
                request.EngineDisplacementUnitOfMeasurementId,
                request.EnginePowerValue,
                request.EnginePowerUnitOfMeasurementId,
                request.WeightValue,
                request.WeightUnitOfMeasurementId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetEngineModelByIdAsync(
        Guid engineModelId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetEngineModelByIdQuery(engineModelId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchEngineModelsAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchEngineModelsQuery(holdingId, search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }
}