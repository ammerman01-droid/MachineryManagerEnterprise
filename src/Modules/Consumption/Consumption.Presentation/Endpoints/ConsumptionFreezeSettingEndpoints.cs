using MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Commands.SetConsumptionFreezeThreshold;
using MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Queries.GetConsumptionFreezeThreshold;
using MachineryManagerEnterprise.Consumption.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Consumption.Presentation.Endpoints;

/// <summary>Maps the Consumption module's freeze-threshold REST endpoints: base path <c>/api/v1/consumption-freeze-setting</c>.</summary>
public static class ConsumptionFreezeSettingEndpoints
{
    /// <summary>Registers the Consumption Freeze Setting endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapConsumptionFreezeSettingEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/consumption-freeze-setting")
            .WithTags("ConsumptionFreezeSetting")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapGet("/", GetConsumptionFreezeThresholdAsync)
            .WithName("GetConsumptionFreezeThreshold")
            .WithSummary("Retrieves the Consumption freeze threshold currently set for an Organization (null if never set).");

        group.MapPut("/", SetConsumptionFreezeThresholdAsync)
            .WithName("SetConsumptionFreezeThreshold")
            .WithSummary("Sets (or moves) the Organization's Consumption freeze threshold. Restricted to an Organization Administrator.");

        return endpoints;
    }

    /// <summary>Handles <c>GET /api/v1/consumption-freeze-setting?organizationId=...</c>.</summary>
    private static async Task<IResult> GetConsumptionFreezeThresholdAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetConsumptionFreezeThresholdQuery(organizationId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/consumption-freeze-setting?organizationId=...</c>.</summary>
    private static async Task<IResult> SetConsumptionFreezeThresholdAsync(
        Guid organizationId,
        SetConsumptionFreezeThresholdRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SetConsumptionFreezeThresholdCommand(organizationId, request.ThresholdDate), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }
}
