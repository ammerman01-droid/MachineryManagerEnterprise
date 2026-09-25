using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RegisterLubricantType;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RenameLubricantType;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.DeleteLubricantType;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Queries.GetLubricantTypeById;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Queries.GetLubricantTypesByHolding;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Lubricant Type REST endpoints: base path <c>/api/v1/lubricant-types</c>.</summary>
public static class LubricantTypeEndpoints
{
    /// <summary>Registers the Lubricant Type endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapLubricantTypeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/lubricant-types")
            .WithTags("LubricantTypes")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterLubricantTypeAsync)
            .WithName("RegisterLubricantType")
            .WithSummary("Registers a new Lubricant Type within a Holding.");

        group.MapGet("/", GetLubricantTypesByHoldingAsync)
            .WithName("GetLubricantTypesByHolding")
            .WithSummary("Retrieves every Lubricant Type registered for a Holding.");

        group.MapPut("/{id:guid}", RenameLubricantTypeAsync)
            .WithName("RenameLubricantType")
            .WithSummary("Renames an existing Lubricant Type.");

        group.MapDelete("/{id:guid}", DeleteLubricantTypeAsync)
            .WithName("DeleteLubricantType")
            .WithSummary("Deletes a Lubricant Type, if not referenced by any Lubricant Overflow Report line.");

        group.MapGet("/{id:guid}", GetLubricantTypeByIdAsync)
            .WithName("GetLubricantTypeById")
            .WithSummary("Retrieves a single Lubricant Type by its identifier.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/lubricant-types</c>.</summary>
    private static async Task<IResult> RegisterLubricantTypeAsync(
        RegisterLubricantTypeRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterLubricantTypeCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/lubricant-types/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/lubricant-types?holdingId=...</c>.</summary>
    private static async Task<IResult> GetLubricantTypesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLubricantTypesByHoldingQuery(holdingId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/lubricant-types/{id}</c>.</summary>
    private static async Task<IResult> RenameLubricantTypeAsync(
        Guid id,
        RegisterLubricantTypeRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RenameLubricantTypeCommand(id, request.Name), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>DELETE /api/v1/lubricant-types/{id}</c>.</summary>
    private static async Task<IResult> DeleteLubricantTypeAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteLubricantTypeCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/lubricant-types/{id}</c>.</summary>
    private static async Task<IResult> GetLubricantTypeByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLubricantTypeByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}
