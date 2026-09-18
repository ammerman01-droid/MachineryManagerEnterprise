using Mapster;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarHistory;

/// <summary>Handles <see cref="GetWorkCalendarHistoryQuery"/>.</summary>
public sealed class GetWorkCalendarHistoryQueryHandler
    : IRequestHandler<GetWorkCalendarHistoryQuery, Result<WorkCalendarHistoryDto>>
{
    private const string RequiredPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _repository;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetWorkCalendarHistoryQueryHandler"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public GetWorkCalendarHistoryQueryHandler(
        IWorkCalendarRepository repository,
        IProjectLookupService projectLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _repository = repository;
        _projectLookupService = projectLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <summary>Executes the history-retrieval use case.</summary>
    /// <param name="request">The query containing the calendar identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="WorkCalendarHistoryDto"/>, or a business error.</returns>
    public async Task<Result<WorkCalendarHistoryDto>> Handle(GetWorkCalendarHistoryQuery request, CancellationToken cancellationToken)
    {
        var calendarId = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure<WorkCalendarHistoryDto>(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<WorkCalendarHistoryDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<WorkCalendarHistoryDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var historicalPatterns = calendar.WorkPatterns
            .Where(p => p.Status is global::WorkCalendar.Domain.WorkPatternStatus.Superseded or global::WorkCalendar.Domain.WorkPatternStatus.Historical)
            .Select(p => _mapper.Map<WorkPatternDto>(p))
            .ToList();

        var cancelledOverrides = calendar.DayOverrides
            .Where(o => o.IsCancelled)
            .Select(o => _mapper.Map<DayOverrideDto>(o))
            .ToList();

        return Result.Success(new WorkCalendarHistoryDto(historicalPatterns, cancelledOverrides));
    }
}