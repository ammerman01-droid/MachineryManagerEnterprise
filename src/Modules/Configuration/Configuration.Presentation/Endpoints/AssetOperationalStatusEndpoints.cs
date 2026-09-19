using MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Commands.RegisterAssetOperationalStatus;
using MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Queries.GetAssetOperationalStatusesByHolding;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Asset Operational Status REST endpoints: base path <c>/api/v1/asset-operational-statuses</c>.</summary>
public static class AssetOperationalStatusEndpoints
{
    /// <summary>Registers the endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapAssetOperationalStatusEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/asset-operational-statuses")
            .WithTags("AssetOperationalStatuses")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterAsync)
            .WithName("RegisterAssetOperationalStatus")
            .WithSummary("Registers a new Asset operational status option within a Holding.");

        group.MapGet("/", GetByHoldingAsync)
            .WithName("GetAssetOperationalStatusesByHolding")
            .WithSummary("Retrieves every Asset operational status option registered for a Holding.");

        return endpoints;
    }

    private static async Task<IResult> RegisterAsync(
        RegisterAssetOperationalStatusRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterAssetOperationalStatusCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/asset-operational-statuses/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAssetOperationalStatusesByHoldingQuery(holdingId), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}