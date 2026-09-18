using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.AddDayOverride;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.AddWorkPattern;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.CancelDayOverride;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RemoveWorkPattern;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RetireWorkPattern;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.SupersedeWorkPattern;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetActiveWorkPatternForDate;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetDayOverride;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarById;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarHistory;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkingCapacityForRange;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkPattern;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.ResolveDaySchedule;
using MachineryManagerEnterprise.WorkCalendar.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using WorkCalendar.Domain;

namespace MachineryManagerEnterprise.WorkCalendar.Presentation.Endpoints;

/// <summary>
/// Maps the WorkCalendar module's REST endpoints per 07-api conventions
/// (Section 8): base path <c>/api/v1/work-calendars</c>.
/// </summary>
public static class WorkCalendarEndpoints
{
    /// <summary>Registers the WorkCalendar endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to register endpoints on.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapWorkCalendarEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/work-calendars")
            .WithTags("WorkCalendars")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapGet("/{workCalendarId:guid}", GetWorkCalendarByIdAsync)
            .WithName("GetWorkCalendarById")
            .WithSummary("Retrieves a Work Calendar with its Work Patterns and Day Overrides.");

        // Redesign (chat, 2026-09-16): a Project's calendar is created
        // transparently on first access — this is now the only entry
        // point the UI needs; it never 404s for a Project that simply
        // hasn't been visited yet.
        group.MapGet("/by-project/{projectId:guid}", GetOrCreateWorkCalendarByProjectIdAsync)
            .WithName("GetWorkCalendarByProjectId")
            .WithSummary("Retrieves the Work Calendar for a Project, transparently creating one if none exists yet.");

        group.MapGet("/{workCalendarId:guid}/history", GetWorkCalendarHistoryAsync)
            .WithName("GetWorkCalendarHistory")
            .WithSummary("Retrieves the full historical audit trail (superseded/historical patterns, cancelled overrides).");

        group.MapGet("/{workCalendarId:guid}/resolve", ResolveDayScheduleAsync)
            .WithName("ResolveDaySchedule")
            .WithSummary("Resolves the effective schedule for a specific calendar date.");

        group.MapGet("/{workCalendarId:guid}/capacity", GetWorkingCapacityForRangeAsync)
            .WithName("GetWorkingCapacityForRange")
            .WithSummary("Computes aggregate working capacity over a date range.");

        // Work Patterns (sub-resource)
        group.MapPost("/{workCalendarId:guid}/work-patterns", AddWorkPatternAsync)
            .WithName("AddWorkPattern")
            .WithSummary("Adds a new Work Pattern to a Work Calendar.");

        group.MapGet("/{workCalendarId:guid}/work-patterns/{workPatternId:guid}", GetWorkPatternAsync)
            .WithName("GetWorkPattern")
            .WithSummary("Retrieves a single Work Pattern.");

        group.MapGet("/{workCalendarId:guid}/work-patterns/active-on", GetActiveWorkPatternForDateAsync)
            .WithName("GetActiveWorkPatternForDate")
            .WithSummary("Retrieves whichever Work Pattern governs a specific calendar date, if any.");

        group.MapDelete("/{workCalendarId:guid}/work-patterns/{workPatternId:guid}", RemoveWorkPatternAsync)
            .WithName("RemoveWorkPattern")
            .WithSummary("Removes a Work Pattern still in Draft.");

        group.MapPost("/{workCalendarId:guid}/work-patterns/{workPatternId:guid}/supersede", SupersedeWorkPatternAsync)
            .WithName("SupersedeWorkPattern")
            .WithSummary("Replaces an Active Work Pattern with a new version.");

        group.MapPost("/{workCalendarId:guid}/work-patterns/{workPatternId:guid}/retire", RetireWorkPatternAsync)
            .WithName("RetireWorkPattern")
            .WithSummary("Manually retires an Active Work Pattern directly to Historical, with no replacement.");

        // Day Overrides (sub-resource)
        group.MapPost("/{workCalendarId:guid}/day-overrides", AddDayOverrideAsync)
            .WithName("AddDayOverride")
            .WithSummary("Adds an explicit Day Override for a specific date.");

        group.MapGet("/{workCalendarId:guid}/day-overrides/{dayOverrideId:guid}", GetDayOverrideAsync)
            .WithName("GetDayOverride")
            .WithSummary("Retrieves a single Day Override.");

        group.MapPost("/{workCalendarId:guid}/day-overrides/{dayOverrideId:guid}/cancel", CancelDayOverrideAsync)
            .WithName("CancelDayOverride")
            .WithSummary("Cancels a Day Override (retained for audit trail, not deleted).");

        return endpoints;
    }

    private static async Task<IResult> GetWorkCalendarByIdAsync(
        Guid workCalendarId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWorkCalendarByIdQuery(workCalendarId), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetOrCreateWorkCalendarByProjectIdAsync(
        Guid projectId,
        ISender sender,
        WorkCalendarApplicationService applicationService,
        MachineryManagerEnterprise.SharedKernel.Abstractions.IProjectLookupService projectLookupService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var projectName = await projectLookupService.GetNameAsync(projectId, cancellationToken);

        if (projectName is null)
        {
            return Results.NotFound(new { message = $"Project with id {projectId} was not found." });
        }

        var ensureResult = await applicationService.EnsureCalendarForProjectAsync(projectId, projectName, cancellationToken);

        if (ensureResult.IsFailure)
        {
            return ensureResult.ToProblemResult(httpContext);
        }

        var result = await sender.Send(new GetWorkCalendarByIdQuery(ensureResult.Value), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetWorkCalendarHistoryAsync(
        Guid workCalendarId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWorkCalendarHistoryQuery(workCalendarId), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ResolveDayScheduleAsync(
        Guid workCalendarId, DateOnly date, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ResolveDayScheduleQuery(workCalendarId, date), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetWorkingCapacityForRangeAsync(
        Guid workCalendarId, DateOnly startDate, DateOnly endDate, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWorkingCapacityForRangeQuery(workCalendarId, startDate, endDate), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> AddWorkPatternAsync(
        Guid workCalendarId, AddWorkPatternRequest request, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var scheduleResult = BuildWeeklySchedule(request.WeeklySchedule);

        if (scheduleResult.IsFailure)
        {
            return scheduleResult.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new AddWorkPatternCommand(workCalendarId, request.StartDate, request.EndDate, scheduleResult.Value),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/work-calendars/{workCalendarId}/work-patterns/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetWorkPatternAsync(
        Guid workCalendarId, Guid workPatternId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWorkPatternQuery(workCalendarId, workPatternId), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetActiveWorkPatternForDateAsync(
        Guid workCalendarId, DateOnly date, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetActiveWorkPatternForDateQuery(workCalendarId, date), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RemoveWorkPatternAsync(
        Guid workCalendarId, Guid workPatternId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RemoveWorkPatternCommand(workCalendarId, workPatternId), cancellationToken);
        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SupersedeWorkPatternAsync(
        Guid workCalendarId, Guid workPatternId, SupersedeWorkPatternRequest request,
        ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var scheduleResult = BuildWeeklySchedule(request.WeeklySchedule);

        if (scheduleResult.IsFailure)
        {
            return scheduleResult.ToProblemResult(httpContext);
        }

        var result = await sender.Send(
            new SupersedeWorkPatternCommand(workCalendarId, workPatternId, request.StartDate, request.EndDate, scheduleResult.Value),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/work-calendars/{workCalendarId}/work-patterns/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> RetireWorkPatternAsync(
        Guid workCalendarId, Guid workPatternId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RetireWorkPatternCommand(workCalendarId, workPatternId), cancellationToken);
        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> AddDayOverrideAsync(
        Guid workCalendarId, AddDayOverrideRequest request, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        DaySchedule? customSchedule = null;

        if (request.CustomSchedule is not null)
        {
            var scheduleResult = BuildDaySchedule(request.CustomSchedule);

            if (scheduleResult.IsFailure)
            {
                return scheduleResult.ToProblemResult(httpContext);
            }

            customSchedule = scheduleResult.Value;
        }

        var result = await sender.Send(
            new AddDayOverrideCommand(workCalendarId, request.Date, request.Type, customSchedule),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/work-calendars/{workCalendarId}/day-overrides/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetDayOverrideAsync(
        Guid workCalendarId, Guid dayOverrideId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDayOverrideQuery(workCalendarId, dayOverrideId), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> CancelDayOverrideAsync(
        Guid workCalendarId, Guid dayOverrideId, ISender sender, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelDayOverrideCommand(workCalendarId, dayOverrideId), cancellationToken);
        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static Result<Dictionary<DayOfWeek, DaySchedule>> BuildWeeklySchedule(
        IReadOnlyDictionary<DayOfWeek, DayScheduleRequest> request)
    {
        var result = new Dictionary<DayOfWeek, DaySchedule>();

        foreach (var (day, scheduleRequest) in request)
        {
            var dayResult = BuildDaySchedule(scheduleRequest);

            if (dayResult.IsFailure)
            {
                return Result.Failure<Dictionary<DayOfWeek, DaySchedule>>(dayResult.Error);
            }

            result[day] = dayResult.Value;
        }

        return Result.Success(result);
    }

    private static Result<DaySchedule> BuildDaySchedule(DayScheduleRequest request)
    {
        if (request.Shifts is null || request.Shifts.Count == 0)
        {
            return Result.Success(DaySchedule.DayOff());
        }

        var shifts = new List<Shift>();

        foreach (var shiftRequest in request.Shifts)
        {
            var breaksResult = new List<Break>();

            foreach (var breakRequest in shiftRequest.Breaks ?? [])
            {
                var breakResult = Break.Create(breakRequest.StartTime, breakRequest.EndTime);

                if (breakResult.IsFailure)
                {
                    return Result.Failure<DaySchedule>(breakResult.Error);
                }

                breaksResult.Add(breakResult.Value);
            }

            var shiftResult = Shift.Create(shiftRequest.StartTime, shiftRequest.EndTime, breaksResult);

            if (shiftResult.IsFailure)
            {
                return Result.Failure<DaySchedule>(shiftResult.Error);
            }

            shifts.Add(shiftResult.Value);
        }

        return DaySchedule.Create(shifts);
    }
}