using Mapster;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkingCapacityForRange;

/// <summary>Handles <see cref="GetWorkingCapacityForRangeQuery"/> via the <see cref="global::WorkCalendar.Domain.WorkCalendarCapacityService"/> Domain Service.</summary>
public sealed class GetWorkingCapacityForRangeQueryHandler
    : IRequestHandler<GetWorkingCapacityForRangeQuery, Result<WorkingCapacityDto>>
{
    private const string RequiredPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _repository;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetWorkingCapacityForRangeQueryHandler"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="unitOfWork">The unit of work, used to persist any lifecycle auto-correction.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project each day's resolution to a DTO.</param>
    public GetWorkingCapacityForRangeQueryHandler(
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

    /// <summary>Executes the capacity-calculation use case.</summary>
    /// <param name="request">The query containing the calendar identifier and date range.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="WorkingCapacityDto"/>, or a business error.</returns>
    public async Task<Result<WorkingCapacityDto>> Handle(GetWorkingCapacityForRangeQuery request, CancellationToken cancellationToken)
    {
        var calendarId = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure<WorkingCapacityDto>(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        if (calendar.SynchronizeLifecycle(DateOnly.FromDateTime(_dateTimeProvider.UtcNow.DateTime)))
        {
            _repository.Update(calendar);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (request.EndDate < request.StartDate)
        {
            return Result.Failure<WorkingCapacityDto>(Error.Validation("WorkCalendar.InvalidDateRange", "End date must not precede start date."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<WorkingCapacityDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<WorkingCapacityDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var totalCapacity = global::WorkCalendar.Domain.WorkCalendarCapacityService.CalculateTotalCapacity(
            calendar, request.StartDate, request.EndDate);

        var dayResolutions = global::WorkCalendar.Domain.WorkCalendarCapacityService.ResolveRange(
            calendar, request.StartDate, request.EndDate);

        var dto = new WorkingCapacityDto(
            request.StartDate,
            request.EndDate,
            totalCapacity,
            dayResolutions.Select(r => _mapper.Map<DayResolutionDto>(r)).ToList());

        return Result.Success(dto);
    }
}