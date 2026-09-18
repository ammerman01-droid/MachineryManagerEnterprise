using Mapster;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application;

/// <summary>
/// Application Service for cross-module and infra-facing consumption of
/// WorkCalendar (10.17/5.7), and for the lazy-provisioning orchestration
/// the Presentation layer's "get calendar for Project" endpoint needs
/// (chat, 2026-09-16 — a Project's calendar is created transparently on
/// first access, never through an explicit user-facing registration step).
/// </summary>
public sealed class WorkCalendarApplicationService
{
    private const string ViewPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _repository;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="WorkCalendarApplicationService"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project domain results to DTOs.</param>
    public WorkCalendarApplicationService(
        IWorkCalendarRepository repository,
        IWorkCalendarUnitOfWork unitOfWork,
        IProjectLookupService projectLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _projectLookupService = projectLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns the existing Work Calendar for a Project, or transparently
    /// creates one (named after the Project) if none exists yet.
    /// </summary>
    /// <param name="projectId">The Project to ensure a calendar exists for.</param>
    /// <param name="projectName">The Project's current name, used only if a new calendar must be created.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the calendar's identifier, or a business error.</returns>
    /// <remarks>
    /// Assumption (chat, 2026-09-16): this does not check any permission
    /// — since the calendar is invisible infrastructure the user never
    /// explicitly asks to create, gating its first-time provisioning
    /// behind a permission would block a View-only user from ever
    /// seeing their Project's calendar page. Flag if this should change.
    /// </remarks>
    public async Task<Result<Guid>> EnsureCalendarForProjectAsync(
        Guid projectId, string projectName, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByProjectIdAsync(projectId, cancellationToken);

        if (existing is not null)
        {
            return Result.Success(existing.Id.Value);
        }

        var result = global::WorkCalendar.Domain.WorkCalendar.Register(projectId, projectName, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _repository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }

    /// <summary>Resolves the effective schedule for a single calendar date.</summary>
    /// <param name="workCalendarId">The Work Calendar to resolve against.</param>
    /// <param name="date">The calendar date to resolve.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the resolved day, or a business error.</returns>
    public async Task<Result<DayResolutionDto>> ResolveDailyScheduleAsync(
        Guid workCalendarId, DateOnly date, CancellationToken cancellationToken = default)
    {
        var authResult = await LoadAndAuthorizeAsync(workCalendarId, cancellationToken);

        if (authResult.IsFailure)
        {
            return Result.Failure<DayResolutionDto>(authResult.Error);
        }

        var resolution = global::WorkCalendar.Domain.WorkCalendarResolutionService.Resolve(authResult.Value, date);

        return Result.Success(_mapper.Map<DayResolutionDto>(resolution));
    }

    /// <summary>Computes aggregate working capacity over a date range.</summary>
    /// <param name="workCalendarId">The Work Calendar to compute against.</param>
    /// <param name="startDate">The range's first date (inclusive).</param>
    /// <param name="endDate">The range's last date (inclusive).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the computed capacity, or a business error.</returns>
    public async Task<Result<WorkingCapacityDto>> CalculateCapacityOverRangeAsync(
        Guid workCalendarId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        var authResult = await LoadAndAuthorizeAsync(workCalendarId, cancellationToken);

        if (authResult.IsFailure)
        {
            return Result.Failure<WorkingCapacityDto>(authResult.Error);
        }

        var days = global::WorkCalendar.Domain.WorkCalendarCapacityService.ResolveRange(authResult.Value, startDate, endDate);
        var totalNetHours = global::WorkCalendar.Domain.WorkCalendarCapacityService.CalculateTotalCapacity(authResult.Value, startDate, endDate);

        return Result.Success(new WorkingCapacityDto(
            startDate, endDate, totalNetHours, _mapper.Map<List<DayResolutionDto>>(days)));
    }

    /// <summary>
    /// Checks, without mutating anything, whether a prospective Work
    /// Pattern date range would overlap an existing one.
    /// </summary>
    /// <param name="workCalendarId">The Work Calendar to check against.</param>
    /// <param name="startDate">The prospective pattern's start date.</param>
    /// <param name="endDate">The prospective pattern's end date.</param>
    /// <param name="excludingPatternId">A pattern to exclude from the check (e.g. the one being superseded).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating whether an overlap would occur, or a business error.</returns>
    public async Task<Result<bool>> ValidatePatternOverlapsAsync(
        Guid workCalendarId, DateOnly startDate, DateOnly endDate, Guid? excludingPatternId, CancellationToken cancellationToken = default)
    {
        var authResult = await LoadAndAuthorizeAsync(workCalendarId, cancellationToken);

        if (authResult.IsFailure)
        {
            return Result.Failure<bool>(authResult.Error);
        }

        var excluding = excludingPatternId is { } id
            ? global::WorkCalendar.Domain.WorkPatternId.From(id)
            : null;

        var wouldOverlap = global::WorkCalendar.Domain.WorkCalendarValidationService.WouldOverlapExistingPattern(
            authResult.Value, startDate, endDate, excluding);

        return Result.Success(wouldOverlap);
    }

    private async Task<Result<global::WorkCalendar.Domain.WorkCalendar>> LoadAndAuthorizeAsync(
        Guid workCalendarId, CancellationToken cancellationToken)
    {
        var id = global::WorkCalendar.Domain.WorkCalendarId.From(workCalendarId);
        var calendar = await _repository.GetByIdAsync(id, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure<global::WorkCalendar.Domain.WorkCalendar>(
                Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {workCalendarId} was not found."));
        }

        if (calendar.SynchronizeLifecycle(DateOnly.FromDateTime(_dateTimeProvider.UtcNow.DateTime)))
        {
            _repository.Update(calendar);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<global::WorkCalendar.Domain.WorkCalendar>(
                global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, ViewPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<global::WorkCalendar.Domain.WorkCalendar>(
                global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        return Result.Success(calendar);
    }
}