using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.CorrectEntryDetails;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.CorrectEntryReading;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.DeleteLatestUsageEntry;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.FreezeUsageUpTo;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RebaseCounter;
using MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetAccumulatedOperationalUsage;
using MachineryManagerEnterprise.Usage.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Usage.Presentation.Endpoints;

/// <summary>
/// Maps the Usage module's Usage Ledger REST endpoints per 07-api
/// conventions (Section 8, mirroring <c>AssetEndpoints</c>): base path
/// <c>/api/v1/usage-ledgers</c>. Every route is addressed by the
/// ledger's natural key (Owner + Unit) rather than by a Meter Device
/// id, since the device that produced a given reading may since have
/// been removed. Registering a new shift's usage lives under
/// <c>MeterDeviceEndpoints</c> instead, since
/// <c>RegisterShiftReadingCommand</c> is addressed by device id.
/// </summary>
public static class DailyUsageEndpoints
{
    /// <summary>Registers the Usage Ledger endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapDailyUsageEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/usage-ledgers/{ownerType}/{ownerId:guid}/{unit}")
            .WithTags("DailyUsage")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPut("/daily-usage/{entryDate}/{shiftIndex:int}", CorrectEntryReadingAsync)
            .WithName("CorrectEntryReading")
            .WithSummary("Corrects an already-registered shift reading's raw value (BR-051), as long as it is not yet frozen (BR-052/BR-053).");

        group.MapPut("/daily-usage/{entryDate}/{shiftIndex:int}/details", CorrectEntryDetailsAsync)
            .WithName("CorrectEntryDetails")
            .WithSummary("Corrects a shift reading's operator attribution and/or shift times, without touching its reading or Operational Usage.");

        group.MapDelete("/daily-usage/{entryDate}", DeleteLatestUsageEntryAsync)
            .WithName("DeleteLatestUsageEntry")
            .WithSummary("Deletes the most recently registered entry — a shift reading or a counter rebase (BR-049 — backward-only; blocked once frozen).");

        group.MapPost("/rebase", RebaseCounterAsync)
            .WithName("RebaseCounter")
            .WithSummary("Explicitly rebases this ledger's counter — the device was replaced or its counter reset.");

        group.MapPost("/freeze", FreezeUsageUpToAsync)
            .WithName("FreezeUsageUpTo")
            .WithSummary("Moves this ledger's freeze boundary forward (BR-052); entries on or before it become immutable to project-level users.");

        group.MapGet("/accumulated-usage", GetAccumulatedOperationalUsageAsync)
            .WithName("GetAccumulatedOperationalUsage")
            .WithSummary("Sums this ledger's Operational Usage across a date range — e.g. since a Periodic Maintenance module's last service date.");

        return endpoints;
    }

    /// <summary>
    /// Parses the <c>{ownerType}</c> and <c>{unit}</c> route segments by
    /// name (not numeric value) — same convention as
    /// <c>AssetEndpoints.ChangeAssetStatusAsync</c> — returning a
    /// validation failure for either segment if it is not recognized.
    /// </summary>
    private static Result<(global::Usage.Domain.UsageOwnerType OwnerType, MeterReadingUnit Unit)> ParseRouteKey(
        string ownerType, string unit)
    {
        if (!Enum.TryParse<global::Usage.Domain.UsageOwnerType>(ownerType, ignoreCase: true, out var parsedOwnerType)
            || !Enum.IsDefined(parsedOwnerType))
        {
            return Result.Failure<(global::Usage.Domain.UsageOwnerType, MeterReadingUnit)>(Error.Validation(
                "UsageLedger.InvalidOwnerType",
                $"'{ownerType}' is not a recognized owner type. Expected 'Asset' or 'Component'."));
        }

        if (!Enum.TryParse<MeterReadingUnit>(unit, ignoreCase: true, out var parsedUnit)
            || !Enum.IsDefined(parsedUnit))
        {
            return Result.Failure<(global::Usage.Domain.UsageOwnerType, MeterReadingUnit)>(Error.Validation(
                "UsageLedger.InvalidUnit",
                $"'{unit}' is not a recognized reading unit. Expected 'Hour', 'Kilometer', or 'Mile'."));
        }

        return Result.Success((parsedOwnerType, parsedUnit));
    }

    private static async Task<IResult> CorrectEntryReadingAsync(
        string ownerType,
        Guid ownerId,
        string unit,
        DateOnly entryDate,
        int shiftIndex,
        CorrectEntryReadingRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var key = ParseRouteKey(ownerType, unit);

        if (key.IsFailure)
        {
            return key.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new CorrectEntryReadingCommand(key.Value.OwnerType, ownerId, key.Value.Unit, entryDate, shiftIndex, request.NewRawReadingValue),
            cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> CorrectEntryDetailsAsync(
        string ownerType,
        Guid ownerId,
        string unit,
        DateOnly entryDate,
        int shiftIndex,
        CorrectEntryDetailsRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var key = ParseRouteKey(ownerType, unit);

        if (key.IsFailure)
        {
            return key.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new CorrectEntryDetailsCommand(
                key.Value.OwnerType, ownerId, key.Value.Unit, entryDate, shiftIndex, request.OperatorId, request.ShiftStartTime, request.ShiftEndTime),
            cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DeleteLatestUsageEntryAsync(
        string ownerType,
        Guid ownerId,
        string unit,
        DateOnly entryDate,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        int? shiftIndex = null)
    {
        var key = ParseRouteKey(ownerType, unit);

        if (key.IsFailure)
        {
            return key.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new DeleteLatestUsageEntryCommand(key.Value.OwnerType, ownerId, key.Value.Unit, entryDate, shiftIndex),
            cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RebaseCounterAsync(
        string ownerType,
        string unit,
        Guid ownerId,
        RebaseCounterRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var key = ParseRouteKey(ownerType, unit);

        if (key.IsFailure)
        {
            return key.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new RebaseCounterCommand(key.Value.OwnerType, ownerId, key.Value.Unit, request.NewMeterDeviceId, request.NewRawReadingValue, request.TargetDate),
            cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> FreezeUsageUpToAsync(
        string ownerType,
        Guid ownerId,
        string unit,
        FreezeUsageUpToRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var key = ParseRouteKey(ownerType, unit);

        if (key.IsFailure)
        {
            return key.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new FreezeUsageUpToCommand(key.Value.OwnerType, ownerId, key.Value.Unit, request.FrozenUpToDate),
            cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetAccumulatedOperationalUsageAsync(
        string ownerType,
        Guid ownerId,
        string unit,
        DateOnly sinceDate,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        DateOnly? throughDate = null)
    {
        var key = ParseRouteKey(ownerType, unit);

        if (key.IsFailure)
        {
            return key.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new GetAccumulatedOperationalUsageQuery(key.Value.OwnerType, ownerId, key.Value.Unit, sinceDate, throughDate),
            cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}
