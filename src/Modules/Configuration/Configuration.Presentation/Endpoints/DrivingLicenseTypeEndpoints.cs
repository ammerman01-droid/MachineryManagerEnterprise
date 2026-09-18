using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RegisterDrivingLicenseType;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Queries.GetDrivingLicenseTypesByHolding;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RenameDrivingLicenseType;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.DeleteDrivingLicenseType;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Queries.GetDrivingLicenseTypeById;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Driving License Type REST endpoints: base path <c>/api/v1/driving-license-types</c>.</summary>
public static class DrivingLicenseTypeEndpoints
{
    /// <summary>Registers the Driving License Type endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapDrivingLicenseTypeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/driving-license-types")
            .WithTags("DrivingLicenseTypes")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterDrivingLicenseTypeAsync)
            .WithName("RegisterDrivingLicenseType")
            .WithSummary("Registers a new Driving License Type within a Holding.");

        group.MapGet("/", GetDrivingLicenseTypesByHoldingAsync)
            .WithName("GetDrivingLicenseTypesByHolding")
            .WithSummary("Retrieves every Driving License Type registered for a Holding.");

        group.MapPut("/{id:guid}", RenameDrivingLicenseTypeAsync)
            .WithName("RenameDrivingLicenseType")
            .WithSummary("Renames an existing Driving License Type.");

        group.MapDelete("/{id:guid}", DeleteDrivingLicenseTypeAsync)
            .WithName("DeleteDrivingLicenseType")
            .WithSummary("Deletes a Driving License Type, if not referenced by any Personnel record.");

        group.MapGet("/{id:guid}", GetDrivingLicenseTypeByIdAsync)
            .WithName("GetDrivingLicenseTypeById")
            .WithSummary("Retrieves a single Driving License Type by its identifier.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/driving-license-types</c>.</summary>
    /// <param name="request">The registration payload (HoldingId, Name).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterDrivingLicenseTypeCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>201 Created</c> with the new entry's id on success; otherwise a standard error body.</returns>
    private static async Task<IResult> RegisterDrivingLicenseTypeAsync(
        RegisterDrivingLicenseTypeRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterDrivingLicenseTypeCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/driving-license-types/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/driving-license-types?holdingId=...</c>.</summary>
    /// <param name="holdingId">The Holding whose catalog should be returned (required query parameter).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetDrivingLicenseTypesByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the list on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetDrivingLicenseTypesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDrivingLicenseTypesByHoldingQuery(holdingId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/driving-license-types/{id}</c>.</summary>
    private static async Task<IResult> RenameDrivingLicenseTypeAsync(
        Guid id,
        RegisterDrivingLicenseTypeRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RenameDrivingLicenseTypeCommand(id, request.Name), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>DELETE /api/v1/driving-license-types/{id}</c>.</summary>
    private static async Task<IResult> DeleteDrivingLicenseTypeAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteDrivingLicenseTypeCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

        /// <summary>Handles <c>GET /api/v1/driving-license-types/{id}</c>.</summary>
    private static async Task<IResult> GetDrivingLicenseTypeByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDrivingLicenseTypeByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}