using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.RegisterOverflowComponent;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.RenameOverflowComponent;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.DeleteOverflowComponent;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Queries.GetOverflowComponentById;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Queries.GetOverflowComponentsByHolding;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Overflow Component REST endpoints: base path <c>/api/v1/overflow-components</c>.</summary>
public static class OverflowComponentEndpoints
{
    /// <summary>Registers the Overflow Component endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapOverflowComponentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/overflow-components")
            .WithTags("OverflowComponents")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterOverflowComponentAsync)
            .WithName("RegisterOverflowComponent")
            .WithSummary("Registers a new Overflow Component within a Holding.");

        group.MapGet("/", GetOverflowComponentsByHoldingAsync)
            .WithName("GetOverflowComponentsByHolding")
            .WithSummary("Retrieves every Overflow Component registered for a Holding.");

        group.MapPut("/{id:guid}", RenameOverflowComponentAsync)
            .WithName("RenameOverflowComponent")
            .WithSummary("Renames an existing Overflow Component.");

        group.MapDelete("/{id:guid}", DeleteOverflowComponentAsync)
            .WithName("DeleteOverflowComponent")
            .WithSummary("Deletes a Overflow Component, if not referenced by any Lubricant Overflow Report line.");

        group.MapGet("/{id:guid}", GetOverflowComponentByIdAsync)
            .WithName("GetOverflowComponentById")
            .WithSummary("Retrieves a single Overflow Component by its identifier.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/overflow-components</c>.</summary>
    private static async Task<IResult> RegisterOverflowComponentAsync(
        RegisterOverflowComponentRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterOverflowComponentCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/overflow-components/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/overflow-components?holdingId=...</c>.</summary>
    private static async Task<IResult> GetOverflowComponentsByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetOverflowComponentsByHoldingQuery(holdingId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/overflow-components/{id}</c>.</summary>
    private static async Task<IResult> RenameOverflowComponentAsync(
        Guid id,
        RegisterOverflowComponentRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RenameOverflowComponentCommand(id, request.Name), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>DELETE /api/v1/overflow-components/{id}</c>.</summary>
    private static async Task<IResult> DeleteOverflowComponentAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteOverflowComponentCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/overflow-components/{id}</c>.</summary>
    private static async Task<IResult> GetOverflowComponentByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetOverflowComponentByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}
