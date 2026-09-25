using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.CancelWorkOrder;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.ConvertWorkOrderToRepair;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.EditWorkOrder;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.RegisterWorkOrder;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.GetWorkOrderById;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.SearchWorkOrders;
using MachineryManagerEnterprise.Maintenance.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Maintenance.Presentation.Endpoints;

/// <summary>
/// Maps the Maintenance module's Work Order REST endpoints per
/// 07-api conventions (Section 8): base path <c>/api/v1/work-orders</c>.
/// </summary>
public static class WorkOrderEndpoints
{
    /// <summary>Registers the Work Order endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapWorkOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/work-orders")
            .WithTags("WorkOrders")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterWorkOrderAsync)
            .WithName("RegisterWorkOrder")
            .WithSummary("Registers a new Work Order for an Asset within an Organization.");

        group.MapGet("/{workOrderId:guid}", GetWorkOrderByIdAsync)
            .WithName("GetWorkOrderById")
            .WithSummary("Retrieves a single Work Order by its identifier.");

        group.MapGet("/", SearchWorkOrdersAsync)
            .WithName("SearchWorkOrders")
            .WithSummary("Searches Work Orders within an Organization, with optional text filtering and pagination.");

        group.MapPut("/{workOrderId:guid}", EditWorkOrderAsync)
            .WithName("EditWorkOrder")
            .WithSummary("Edits an Open Work Order's details.");

        group.MapPost("/{workOrderId:guid}/cancel", CancelWorkOrderAsync)
            .WithName("CancelWorkOrder")
            .WithSummary("Cancels an Open Work Order, with a required reason.");

        group.MapPost("/{workOrderId:guid}/convert-to-repair", ConvertWorkOrderToRepairAsync)
            .WithName("ConvertWorkOrderToRepair")
            .WithSummary("Converts an Open Work Order to repair. No approval required.");

        return endpoints;
    }

    private static async Task<IResult> RegisterWorkOrderAsync(
        RegisterWorkOrderRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterWorkOrderCommand(
                request.OrganizationId,
                request.AssetId,
                request.ReportedAt,
                request.ResultingAssetStatus,
                request.MeterReading,
                request.Priority,
                request.RepairType,
                request.ResponsiblePersonnelId,
                request.ObservationDescription,
                request.PredictedRepairLocation,
                request.PartNeedingRepair),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/work-orders/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetWorkOrderByIdAsync(
        Guid workOrderId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWorkOrderByIdQuery(workOrderId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchWorkOrdersAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchWorkOrdersQuery(organizationId, search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> EditWorkOrderAsync(
        Guid workOrderId,
        EditWorkOrderRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new EditWorkOrderCommand(
                workOrderId,
                request.ReportedAt,
                request.ResultingAssetStatus,
                request.MeterReading,
                request.Priority,
                request.RepairType,
                request.ResponsiblePersonnelId,
                request.ObservationDescription,
                request.PredictedRepairLocation,
                request.PartNeedingRepair),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> CancelWorkOrderAsync(
        Guid workOrderId,
        CancelWorkOrderRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelWorkOrderCommand(workOrderId, request.Reason), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ConvertWorkOrderToRepairAsync(
        Guid workOrderId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ConvertWorkOrderToRepairCommand(workOrderId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }
}
