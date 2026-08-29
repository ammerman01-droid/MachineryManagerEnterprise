using MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;
using MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Queries.GetUnitsOfMeasurementByOrganization;
using MachineryManager.Asset.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Asset.Presentation.Endpoints;

/// <summary>Maps the Asset module's Unit of Measurement REST endpoints: base path <c>/api/v1/units-of-measurement</c>.</summary>
public static class UnitOfMeasurementEndpoints
{
    /// <summary>Registers the Unit of Measurement endpoints on the application's route builder.</summary>
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
            .WithSummary("Registers a new Unit of Measurement within an Organization.");

        group.MapGet("/", GetUnitsOfMeasurementByOrganizationAsync)
            .WithName("GetUnitsOfMeasurementByOrganization")
            .WithSummary("Retrieves every Unit of Measurement registered for an Organization.");

        return endpoints;
    }

    private static async Task<IResult> RegisterUnitOfMeasurementAsync(
        RegisterUnitOfMeasurementRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterUnitOfMeasurementCommand(request.OrganizationId, request.Name, request.Category),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/units-of-measurement/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetUnitsOfMeasurementByOrganizationAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUnitsOfMeasurementByOrganizationQuery(organizationId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }
}