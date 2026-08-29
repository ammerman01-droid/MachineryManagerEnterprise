using MachineryManager.Asset.Application.Features.Assets.Commands.ActivateAsset;
using MachineryManager.Asset.Application.Features.Assets.Commands.CommissionAsset;
using MachineryManager.Asset.Application.Features.Assets.Commands.DeactivateAsset;
using MachineryManager.Asset.Application.Features.Assets.Commands.DisposeAsset;
using MachineryManager.Asset.Application.Features.Assets.Commands.RegisterAsset;
using MachineryManager.Asset.Application.Features.Assets.Commands.RetireAsset;
using MachineryManager.Asset.Application.Features.Assets.Queries.GetAssetById;
using MachineryManager.Asset.Application.Features.Assets.Queries.SearchAssets;
using MachineryManager.Asset.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Asset.Presentation.Endpoints;

/// <summary>
/// Maps the Asset module's Asset REST endpoints per 07-api conventions
/// (Section 8): base path <c>/api/v1/assets</c>.
/// </summary>
public static class AssetEndpoints
{
    /// <summary>Registers the Asset endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/assets")
            .WithTags("Assets")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterAssetAsync)
            .WithName("RegisterAsset")
            .WithSummary("Registers a new Asset within an Organization.");

        group.MapGet("/{assetId:guid}", GetAssetByIdAsync)
            .WithName("GetAssetById")
            .WithSummary("Retrieves a single Asset by its identifier.");

        group.MapGet("/", SearchAssetsAsync)
            .WithName("SearchAssets")
            .WithSummary("Searches Assets within an Organization, with optional text filtering and pagination.");

        group.MapPost("/{assetId:guid}/commission", CommissionAssetAsync)
            .WithName("CommissionAsset")
            .WithSummary("Completes commissioning of an Asset (Registered → Commissioned).");

        group.MapPost("/{assetId:guid}/activate", ActivateAssetAsync)
            .WithName("ActivateAsset")
            .WithSummary("Places an Asset into operation (Commissioned or Inactive → Operational).");

        group.MapPost("/{assetId:guid}/deactivate", DeactivateAssetAsync)
            .WithName("DeactivateAsset")
            .WithSummary("Temporarily takes an Asset out of use (Operational → Inactive).");

        group.MapPost("/{assetId:guid}/retire", RetireAssetAsync)
            .WithName("RetireAsset")
            .WithSummary("Permanently withdraws an Asset from use (Operational or Inactive → Retired).");

        group.MapPost("/{assetId:guid}/dispose", DisposeAssetAsync)
            .WithName("DisposeAsset")
            .WithSummary("Marks a Retired Asset as physically disposed of (final state).");

        return endpoints;
    }

    private static async Task<IResult> RegisterAssetAsync(
        RegisterAssetRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterAssetCommand(
                request.OrganizationId,
                request.Code,
                request.AssetModelId,
                request.Color,
                request.SerialNumber,
                request.LicensePlate,
                request.ManufactureYear),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/assets/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetAssetByIdAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAssetByIdQuery(assetId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchAssetsAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchAssetsQuery(organizationId, search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> CommissionAssetAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CommissionAssetCommand(assetId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ActivateAssetAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateAssetCommand(assetId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DeactivateAssetAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateAssetCommand(assetId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RetireAssetAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RetireAssetCommand(assetId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DisposeAssetAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DisposeAssetCommand(assetId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }
}