using MachineryManager.Configuration.Application.Features.UnitCategories.Commands.RegisterUnitCategory;
using MachineryManager.Configuration.Application.Features.UnitCategories.Queries.GetUnitCategoriesByHolding;
using MachineryManager.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Configuration.Presentation.Endpoints;

/// <summary>
/// Maps the Configuration module's Unit Category REST endpoints:
/// base path <c>/api/v1/unit-categories</c>.
/// </summary>
public static class UnitCategoryEndpoints
{
    /// <summary>Registers the Unit Category endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapUnitCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/unit-categories")
            .WithTags("UnitCategories")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterUnitCategoryAsync)
            .WithName("RegisterUnitCategory")
            .WithSummary("Registers a new Unit Category within a Holding.");

        group.MapGet("/", GetUnitCategoriesByHoldingAsync)
            .WithName("GetUnitCategoriesByHolding")
            .WithSummary("Retrieves every Unit Category registered for a Holding.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/unit-categories</c>.</summary>
    /// <param name="request">The registration payload (HoldingId, Name).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterUnitCategoryCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>201 Created</c> with the new category's id on success; otherwise a standard error body.</returns>
    private static async Task<IResult> RegisterUnitCategoryAsync(
        RegisterUnitCategoryRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterUnitCategoryCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/unit-categories/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/unit-categories?holdingId=...</c>.</summary>
    /// <param name="holdingId">The Holding whose category list should be returned (required query parameter).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetUnitCategoriesByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the list of categories on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetUnitCategoriesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUnitCategoriesByHoldingQuery(holdingId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}