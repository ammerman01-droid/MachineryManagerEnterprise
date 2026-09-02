using MachineryManager.Asset.Application.Features.AssetModels.Commands.AssignCompatibleEngineModel;
using MachineryManager.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;
using MachineryManager.Asset.Application.Features.AssetModels.Commands.RemoveCompatibleEngineModel;
using MachineryManager.Asset.Application.Features.AssetModels.Commands.RenameAssetModel;
using MachineryManager.Asset.Application.Features.AssetModels.Queries.GetAssetModelById;
using MachineryManager.Asset.Application.Features.AssetModels.Queries.SearchAssetModels;
using MachineryManager.Asset.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Asset.Presentation.Endpoints;

/// <summary>
/// Maps the Asset module's Asset Model REST endpoints per 07-api
/// conventions (Section 8): base path <c>/api/v1/asset-models</c>.
/// </summary>
public static class AssetModelEndpoints
{
    /// <summary>Registers the Asset Model endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapAssetModelEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/asset-models")
            .WithTags("AssetModels")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterAssetModelAsync)
            .WithName("RegisterAssetModel")
            .WithSummary("Registers a new Asset Model within a Holding.");

        group.MapPut("/{assetModelId:guid}", RenameAssetModelAsync)
            .WithName("RenameAssetModel")
            .WithSummary("Renames an existing Asset Model.");

        group.MapGet("/{assetModelId:guid}", GetAssetModelByIdAsync)
            .WithName("GetAssetModelById")
            .WithSummary("Retrieves a single Asset Model by its identifier.");

        group.MapGet("/", SearchAssetModelsAsync)
            .WithName("SearchAssetModels")
            .WithSummary("Searches Asset Models within a Holding, with optional text filtering and pagination.");

        group.MapPost("/{assetModelId:guid}/compatible-engine-models", AssignCompatibleEngineModelAsync)
            .WithName("AssignCompatibleEngineModel")
            .WithSummary("Marks an Engine Model as compatible with this Asset Model.");

        group.MapDelete("/{assetModelId:guid}/compatible-engine-models/{engineModelId:guid}", RemoveCompatibleEngineModelAsync)
            .WithName("RemoveCompatibleEngineModel")
            .WithSummary("Removes a previously assigned Engine Model compatibility.");

        return endpoints;
    }

    private static async Task<IResult> RegisterAssetModelAsync(
        RegisterAssetModelRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterAssetModelCommand(request.HoldingId, request.Name, request.CompanyId),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/asset-models/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RenameAssetModelAsync(
        Guid assetModelId,
        RenameAssetModelRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RenameAssetModelCommand(assetModelId, request.Name),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetAssetModelByIdAsync(
        Guid assetModelId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAssetModelByIdQuery(assetModelId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchAssetModelsAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchAssetModelsQuery(holdingId, search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> AssignCompatibleEngineModelAsync(
        Guid assetModelId,
        AssignCompatibleEngineModelRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new AssignCompatibleEngineModelCommand(assetModelId, request.EngineModelId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RemoveCompatibleEngineModelAsync(
        Guid assetModelId,
        Guid engineModelId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RemoveCompatibleEngineModelCommand(assetModelId, engineModelId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }
}