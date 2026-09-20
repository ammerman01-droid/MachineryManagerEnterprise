using MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.ChangeAssetStatus;
using MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.RegisterAsset;
using MachineryManagerEnterprise.Asset.Application.Features.Assets.Queries.GetAssetAuthorizedScope;
using MachineryManagerEnterprise.Asset.Application.Features.Assets.Queries.GetAssetById;
using MachineryManagerEnterprise.Asset.Application.Features.Assets.Queries.SearchAssets;
using MachineryManagerEnterprise.Asset.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.UpdateAsset;

namespace MachineryManagerEnterprise.Asset.Presentation.Endpoints;

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

        group.MapGet("/authorized-scope", GetAssetAuthorizedScopeAsync)
            .WithName("GetAssetAuthorizedScope")
            .WithSummary("Resolves the current user's authorized scope for the \"Asset.View\" permission.");

        group.MapGet("/{assetId:guid}", GetAssetByIdAsync)
            .WithName("GetAssetById")
            .WithSummary("Retrieves a single Asset by its identifier.");

        group.MapGet("/", SearchAssetsAsync)
            .WithName("SearchAssets")
            .WithSummary("Searches Assets within an Organization, with optional text filtering and pagination.");

        group.MapPut("/{assetId:guid}/status", ChangeAssetStatusAsync)
            .WithName("ChangeAssetStatus")
            .WithSummary("Moves an Asset to any other status: Active, Ready, OutOfService, or OutOfFleet.");

        
        group.MapPut("/{assetId:guid}", UpdateAssetAsync)
    .WithName("UpdateAsset")
    .WithSummary("Updates an existing Asset's mutable details.");

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
                request.Name,
                request.AssetModelId,
                request.ColorId,
                request.SerialNumber,
                request.ChassisNumber,
                request.BodyNumber,
                request.Vin,
                request.LicensePlate,
                request.ManufactureYear,
                request.ProjectId,
                request.MeterReadingUnit,
                request.PrimaryFuelKind,
                request.PrimaryFuelUnit,
                request.SecondaryFuelKind,
                request.SecondaryFuelUnit),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/assets/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetAssetAuthorizedScopeAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAssetAuthorizedScopeQuery(), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
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

    private static async Task<IResult> ChangeAssetStatusAsync(
        Guid assetId,
        ChangeAssetStatusRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        // The status is accepted by name (as the API returns it), not by its numeric value.
        if (!Enum.TryParse<global::Asset.Domain.AssetStatus>(request.Status, ignoreCase: true, out var newStatus)
            || !Enum.IsDefined(newStatus))
        {
            return global::MachineryManagerEnterprise.SharedKernel.Result
                .Failure(global::Asset.Domain.AssetErrors.InvalidStatus(request.Status))
                .ToProblemResult(httpContext);
        }

        var result = await sender.Send(new ChangeAssetStatusCommand(assetId, newStatus), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> UpdateAssetAsync(
    Guid assetId,
    UpdateAssetRequest request,
    ISender sender,
    HttpContext httpContext,
    CancellationToken cancellationToken)
{
    var result = await sender.Send(
        new UpdateAssetCommand(
            assetId,
            request.Code,
            request.Name,
            request.AssetModelId,
            request.ColorId,
            request.ProjectId,
            request.SerialNumber,
            request.ChassisNumber,
            request.BodyNumber,
            request.Vin,
            request.LicensePlate,
            request.ManufactureYear,
            request.MeterReadingUnit,
            request.PrimaryFuelKind,
            request.PrimaryFuelUnit,
            request.SecondaryFuelKind,
            request.SecondaryFuelUnit),
        cancellationToken);

    return result.IsSuccess
        ? Results.NoContent()
        : result.ToProblemResult(httpContext);
}
}
