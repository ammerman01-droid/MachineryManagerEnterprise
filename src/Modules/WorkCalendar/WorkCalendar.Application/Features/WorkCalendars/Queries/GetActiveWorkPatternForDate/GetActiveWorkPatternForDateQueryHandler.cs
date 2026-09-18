using Mapster;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetActiveWorkPatternForDate;

/// <summary>Handles <see cref="GetActiveWorkPatternForDateQuery"/>.</summary>
public sealed class GetActiveWorkPatternForDateQueryHandler
    : IRequestHandler<GetActiveWorkPatternForDateQuery, Result<WorkPatternDto?>>
{
    private const string RequiredPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _repository;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetActiveWorkPatternForDateQueryHandler"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetActiveWorkPatternForDateQueryHandler(
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

    /// <summary>Executes the lookup use case.</summary>
    /// <param name="request">The query containing the calendar identifier and target date.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the governing <see cref="WorkPatternDto"/>, or <see langword="null"/> if none covers the date.</returns>
    public async Task<Result<WorkPatternDto?>> Handle(GetActiveWorkPatternForDateQuery request, CancellationToken cancellationToken)
    {
        var calendarId = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure<WorkPatternDto?>(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<WorkPatternDto?>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<WorkPatternDto?>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var pattern = calendar.WorkPatterns.FirstOrDefault(
            p => p.Status == global::WorkCalendar.Domain.WorkPatternStatus.Active && p.Covers(request.Date));

        return Result.Success(pattern is null ? null : _mapper.Map<WorkPatternDto>(pattern));
    }
}