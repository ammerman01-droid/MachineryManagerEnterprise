using MachineryManager.Organization.Application.Features.Holdings.Commands.RegisterHolding;
using MachineryManager.Organization.Application.Features.Holdings.Queries.GetHoldingById;
using MachineryManager.Organization.Application.Features.Holdings.Queries.SearchHoldings;
using MachineryManager.Organization.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManager.Organization.Application.Features.Holdings.Commands.RenameHolding;

namespace MachineryManager.Organization.Presentation.Endpoints;

/// <summary>
/// Maps the Holding module's REST endpoints per 07-api conventions.
/// </summary>
public static class HoldingEndpoints
{
    /// <summary>
    /// Registers the Holding module's endpoints on the application's route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapHoldingEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/holdings")
            .WithTags("Holdings")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterHoldingAsync)
            .WithName("RegisterHolding")
            .WithSummary("Registers a new Holding.");

                group.MapPut("/{holdingId:guid}", RenameHoldingAsync)
            .WithName("RenameHolding")
            .WithSummary("Renames an existing Holding.");

        group.MapGet("/{holdingId:guid}", GetHoldingByIdAsync)
            .WithName("GetHoldingById")
            .WithSummary("Retrieves a single Holding by its identifier.");

        group.MapGet("/", SearchHoldingsAsync)
            .WithName("SearchHoldings")
            .WithSummary("Searches Holdings with optional text filtering and pagination.");

        return endpoints;
    }

    private static async Task<IResult> RegisterHoldingAsync(
        RegisterHoldingRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterHoldingCommand(request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/holdings/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetHoldingByIdAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetHoldingByIdQuery(holdingId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchHoldingsAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchHoldingsQuery(search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

        private static async Task<IResult> RenameHoldingAsync(
        Guid holdingId,
        RenameHoldingRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RenameHoldingCommand(holdingId, request.Name),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }
}