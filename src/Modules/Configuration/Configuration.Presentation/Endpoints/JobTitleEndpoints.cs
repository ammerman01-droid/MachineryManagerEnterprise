using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.RegisterJobTitle;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Queries.GetJobTitlesByHolding;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.RenameJobTitle;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.DeleteJobTitle;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Queries.GetJobTitleById;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Job Title REST endpoints: base path <c>/api/v1/job-titles</c>.</summary>
public static class JobTitleEndpoints
{
    /// <summary>Registers the Job Title endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapJobTitleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/job-titles")
            .WithTags("JobTitles")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterJobTitleAsync)
            .WithName("RegisterJobTitle")
            .WithSummary("Registers a new Job Title within a Holding.");

        group.MapGet("/", GetJobTitlesByHoldingAsync)
            .WithName("GetJobTitlesByHolding")
            .WithSummary("Retrieves every Job Title registered for a Holding.");

        group.MapPut("/{id:guid}", RenameJobTitleAsync)
            .WithName("RenameJobTitle")
            .WithSummary("Renames an existing JobTitle.");

        group.MapDelete("/{id:guid}", DeleteJobTitleAsync)
            .WithName("DeleteJobTitle")
            .WithSummary("Deletes a Job Title, if not referenced by any Personnel record.");

        group.MapGet("/{id:guid}", GetJobTitleByIdAsync)
            .WithName("GetJobTitleById")
            .WithSummary("Retrieves a single Job Title by its identifier.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/job-titles</c>.</summary>
    /// <param name="request">The registration payload (HoldingId, Name).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterJobTitleCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>201 Created</c> with the new entry's id on success; otherwise a standard error body.</returns>
    private static async Task<IResult> RegisterJobTitleAsync(
        RegisterJobTitleRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterJobTitleCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/job-titles/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/job-titles?holdingId=...</c>.</summary>
    /// <param name="holdingId">The Holding whose catalog should be returned (required query parameter).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetJobTitlesByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the list on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetJobTitlesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetJobTitlesByHoldingQuery(holdingId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/driving-license-types/{id}</c>.</summary>
    private static async Task<IResult> RenameJobTitleAsync(
        Guid id,
        RegisterJobTitleRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RenameJobTitleCommand(id, request.Name), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>DELETE /api/v1/driving-license-types/{id}</c>.</summary>
    private static async Task<IResult> DeleteJobTitleAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteJobTitleCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

        /// <summary>Handles <c>GET /api/v1/job-titles/{id}</c>.</summary>
    private static async Task<IResult> GetJobTitleByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetJobTitleByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}