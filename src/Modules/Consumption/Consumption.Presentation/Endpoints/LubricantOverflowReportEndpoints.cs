using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.CreateLubricantOverflowReport;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.DeleteLubricantOverflowReport;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.UpdateLubricantOverflowReport;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Queries.GetLubricantOverflowReportById;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Queries.GetLubricantOverflowReportsByAsset;
using MachineryManagerEnterprise.Consumption.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Consumption.Presentation.Endpoints;

/// <summary>Maps the Consumption module's Lubricant Overflow Report REST endpoints: base path <c>/api/v1/lubricant-overflow-reports</c>.</summary>
public static class LubricantOverflowReportEndpoints
{
    /// <summary>Registers the Lubricant Overflow Report endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapLubricantOverflowReportEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/lubricant-overflow-reports")
            .WithTags("LubricantOverflowReports")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", CreateLubricantOverflowReportAsync)
            .WithName("CreateLubricantOverflowReport")
            .WithSummary("Creates a new Lubricant Overflow Report.");

        group.MapGet("/", GetLubricantOverflowReportsByAssetAsync)
            .WithName("GetLubricantOverflowReportsByAsset")
            .WithSummary("Retrieves the Lubricant Overflow Reports recorded for a given Asset.");

        group.MapGet("/{id:guid}", GetLubricantOverflowReportByIdAsync)
            .WithName("GetLubricantOverflowReportById")
            .WithSummary("Retrieves a single Lubricant Overflow Report by its identifier.");

        group.MapPut("/{id:guid}", UpdateLubricantOverflowReportAsync)
            .WithName("UpdateLubricantOverflowReport")
            .WithSummary("Updates an existing Lubricant Overflow Report's date, hour meter reading, lines, and personnel entries.");

        group.MapDelete("/{id:guid}", DeleteLubricantOverflowReportAsync)
            .WithName("DeleteLubricantOverflowReport")
            .WithSummary("Deletes a Lubricant Overflow Report, provided its date has not been frozen.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/lubricant-overflow-reports</c>.</summary>
    private static async Task<IResult> CreateLubricantOverflowReportAsync(
        CreateLubricantOverflowReportRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new CreateLubricantOverflowReportCommand(
            request.AssetId,
            request.ProjectId,
            request.ReportDate,
            request.HourMeterReading,
            request.Lines.Select(l => new LubricantOverflowLineInput(l.OverflowComponentId, l.LubricantTypeId, l.AmountInLiters, l.Reason)).ToList(),
            request.PersonnelEntries.Select(e => new LubricantOverflowReportPersonnelEntryInput(e.PersonnelId, e.StartTime, e.Duration)).ToList());

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/lubricant-overflow-reports/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/lubricant-overflow-reports?assetId=...</c>.</summary>
    private static async Task<IResult> GetLubricantOverflowReportsByAssetAsync(
        Guid assetId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLubricantOverflowReportsByAssetQuery(assetId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/lubricant-overflow-reports/{id}</c>.</summary>
    private static async Task<IResult> GetLubricantOverflowReportByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLubricantOverflowReportByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/lubricant-overflow-reports/{id}</c>.</summary>
    private static async Task<IResult> UpdateLubricantOverflowReportAsync(
        Guid id,
        UpdateLubricantOverflowReportRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLubricantOverflowReportCommand(
            id,
            request.ReportDate,
            request.HourMeterReading,
            request.Lines.Select(l => new LubricantOverflowLineInput(l.OverflowComponentId, l.LubricantTypeId, l.AmountInLiters, l.Reason)).ToList(),
            request.PersonnelEntries.Select(e => new LubricantOverflowReportPersonnelEntryInput(e.PersonnelId, e.StartTime, e.Duration)).ToList());

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>DELETE /api/v1/lubricant-overflow-reports/{id}</c>.</summary>
    private static async Task<IResult> DeleteLubricantOverflowReportAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteLubricantOverflowReportCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }
}
