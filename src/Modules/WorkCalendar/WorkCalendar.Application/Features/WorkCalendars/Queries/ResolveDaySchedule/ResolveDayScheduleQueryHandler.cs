using Mapster;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.ResolveDaySchedule;

/// <summary>Handles <see cref="ResolveDayScheduleQuery"/> by invoking <see cref="global::WorkCalendar.Domain.WorkCalendar.ResolveDay"/> (BR-018-009: sole authority for available hours/day).</summary>
public sealed class ResolveDayScheduleQueryHandler : IRequestHandler<ResolveDayScheduleQuery, Result<DayResolutionDto>>
{
    private const string RequiredPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _repository;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="ResolveDayScheduleQueryHandler"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="unitOfWork">The unit of work, used to persist any lifecycle auto-correction.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the resolution result to a DTO.</param>
    public ResolveDayScheduleQueryHandler(
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

    /// <summary>Executes the resolution use case.</summary>
    /// <param name="request">The query containing the calendar identifier and target date.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="DayResolutionDto"/>, or a business error.</returns>
    public async Task<Result<DayResolutionDto>> Handle(ResolveDayScheduleQuery request, CancellationToken cancellationToken)
    {
        var calendarId = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure<DayResolutionDto>(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        if (calendar.SynchronizeLifecycle(DateOnly.FromDateTime(_dateTimeProvider.UtcNow.DateTime)))
        {
            _repository.Update(calendar);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<DayResolutionDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<DayResolutionDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var resolution = calendar.ResolveDay(request.Date);

        return Result.Success(_mapper.Map<DayResolutionDto>(resolution));
    }
}