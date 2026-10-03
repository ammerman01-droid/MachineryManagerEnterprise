using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RegisterShiftReading;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetDaySchedule;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListMeterDeviceUsageEntries;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.ArchiveMeterDevice;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.InstallMeterDevice;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RecoverMeterDevice;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RegisterMeterDevice;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RemoveMeterDevice;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.ReportMeterDeviceFailure;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.GetMeterDeviceById;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.SearchMeterDevices;
using MachineryManagerEnterprise.Usage.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Usage.Presentation.Endpoints;

/// <summary>
/// Maps the Usage module's Meter Device REST endpoints per 07-api
/// conventions (Section 8, mirroring <c>AssetEndpoints</c>): base path
/// <c>/api/v1/meter-devices</c>.
/// </summary>
public static class MeterDeviceEndpoints
{
    /// <summary>Registers the Meter Device endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapMeterDeviceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/meter-devices")
            .WithTags("MeterDevices")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapGet("/", SearchMeterDevicesAsync)
            .WithName("SearchMeterDevices")
            .WithSummary("Lists an Organization's Meter Devices, paged.");

        group.MapGet("/{meterDeviceId:guid}", GetMeterDeviceByIdAsync)
            .WithName("GetMeterDeviceById")
            .WithSummary("Fetches a single Meter Device by id.");

        group.MapPost("/", RegisterMeterDeviceAsync)
            .WithName("RegisterMeterDevice")
            .WithSummary("Registers a new Meter Device within an Organization (BR-041), with a fixed reading unit.");

        group.MapPost("/{meterDeviceId:guid}/install", InstallMeterDeviceAsync)
            .WithName("InstallMeterDevice")
            .WithSummary("Mounts a registered Meter Device on an owner (an Asset or a Tracked Component).");

        group.MapPost("/{meterDeviceId:guid}/report-failure", ReportMeterDeviceFailureAsync)
            .WithName("ReportMeterDeviceFailure")
            .WithSummary("Marks an installed Meter Device as malfunctioning (BR-014); it stops accepting readings until recovered.");

        group.MapPost("/{meterDeviceId:guid}/recover", RecoverMeterDeviceAsync)
            .WithName("RecoverMeterDevice")
            .WithSummary("Marks a previously failed Meter Device as repaired and ready to resume readings.");

        group.MapPost("/{meterDeviceId:guid}/remove", RemoveMeterDeviceAsync)
            .WithName("RemoveMeterDevice")
            .WithSummary("Removes a Meter Device from its current owner, preserving its unit and reading history (BR-011/BR-013).");

        group.MapPost("/{meterDeviceId:guid}/archive", ArchiveMeterDeviceAsync)
            .WithName("ArchiveMeterDevice")
            .WithSummary("Permanently retires a removed Meter Device from the fleet.");

        group.MapPost("/{meterDeviceId:guid}/shift-reading", RegisterShiftReadingAsync)
            .WithName("RegisterShiftReading")
            .WithSummary("Registers a reading for one Work Calendar shift on this Meter Device's current owner — the primary end-user workflow for this module.");

        group.MapGet("/{meterDeviceId:guid}/day-schedule", GetDayScheduleAsync)
            .WithName("GetMeterDeviceDaySchedule")
            .WithSummary("Resolves the Work Calendar day schedule for this device's owning Asset's current Project — backs the shift picker in the registration dialog.");

        group.MapGet("/{meterDeviceId:guid}/usage-entries", ListMeterDeviceUsageEntriesAsync)
            .WithName("ListMeterDeviceUsageEntries")
            .WithSummary("Lists this device's full usage history across every owner it has ever had, filterable by date range and Project (BR-054).");

        return endpoints;
    }

    private static async Task<IResult> SearchMeterDevicesAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 50)
    {
        var result = await sender.Send(new SearchMeterDevicesQuery(organizationId, page, pageSize), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetMeterDeviceByIdAsync(
        Guid meterDeviceId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetMeterDeviceByIdQuery(meterDeviceId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RegisterMeterDeviceAsync(
        RegisterMeterDeviceRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        // The unit is accepted by name, not by its numeric value — same convention as
        // AssetEndpoints.ChangeAssetStatusAsync / InstallMeterDeviceAsync below.
        if (!Enum.TryParse<MeterReadingUnit>(request.Unit, ignoreCase: true, out var unit)
            || !Enum.IsDefined(unit))
        {
            return Result.Failure(Error.Validation(
                    "MeterDevice.InvalidUnit",
                    $"'{request.Unit}' is not a recognized reading unit. Expected 'Hour', 'Kilometer', or 'Mile'."))
                .ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new RegisterMeterDeviceCommand(request.OrganizationId, unit, request.DailyCapOverride),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/meter-devices/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> InstallMeterDeviceAsync(
        Guid meterDeviceId,
        InstallMeterDeviceRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        // The owner type is accepted by name, not by its numeric value — same convention as
        // AssetEndpoints.ChangeAssetStatusAsync.
        if (!Enum.TryParse<global::Usage.Domain.UsageOwnerType>(request.OwnerType, ignoreCase: true, out var ownerType)
            || !Enum.IsDefined(ownerType))
        {
            return Result.Failure(Error.Validation(
                    "MeterDevice.InvalidOwnerType",
                    $"'{request.OwnerType}' is not a recognized owner type. Expected 'Asset' or 'Component'."))
                .ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new InstallMeterDeviceCommand(meterDeviceId, ownerType, request.OwnerId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ReportMeterDeviceFailureAsync(
        Guid meterDeviceId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ReportMeterDeviceFailureCommand(meterDeviceId), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RecoverMeterDeviceAsync(
        Guid meterDeviceId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RecoverMeterDeviceCommand(meterDeviceId), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RemoveMeterDeviceAsync(
        Guid meterDeviceId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RemoveMeterDeviceCommand(meterDeviceId), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ArchiveMeterDeviceAsync(
        Guid meterDeviceId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ArchiveMeterDeviceCommand(meterDeviceId), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RegisterShiftReadingAsync(
        Guid meterDeviceId,
        RegisterShiftReadingRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterShiftReadingCommand(
                meterDeviceId, request.EntryDate, request.ShiftIndex, request.RawReadingValue, request.OperatorId),
            cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetDayScheduleAsync(
        Guid meterDeviceId,
        DateOnly date,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDayScheduleQuery(meterDeviceId, date), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ListMeterDeviceUsageEntriesAsync(
        Guid meterDeviceId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        Guid? projectId = null,
        int page = 1,
        int pageSize = 50)
    {
        var result = await sender.Send(
            new ListMeterDeviceUsageEntriesQuery(meterDeviceId, fromDate, toDate, projectId, page, pageSize),
            cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}
