using Mapster;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetDayOverride;

/// <summary>Handles <see cref="GetDayOverrideQuery"/>.</summary>
public sealed class GetDayOverrideQueryHandler : IRequestHandler<GetDayOverrideQuery, Result<DayOverrideDto>>
{
    private const string RequiredPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _repository;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetDayOverrideQueryHandler"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetDayOverrideQueryHandler(
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
    /// <param name="request">The query containing the calendar and override identifiers.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="DayOverrideDto"/>, or a business error.</returns>
    public async Task<Result<DayOverrideDto>> Handle(GetDayOverrideQuery request, CancellationToken cancellationToken)
    {
        var calendarId = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure<DayOverrideDto>(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<DayOverrideDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<DayOverrideDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var overrideId = global::WorkCalendar.Domain.DayOverrideId.From(request.DayOverrideId);
        var dayOverride = calendar.DayOverrides.FirstOrDefault(o => o.Id == overrideId);

        if (dayOverride is null)
        {
            return Result.Failure<DayOverrideDto>(global::WorkCalendar.Domain.WorkCalendarErrors.DayOverrideNotFound(request.DayOverrideId));
        }

        return Result.Success(_mapper.Map<DayOverrideDto>(dayOverride));
    }
}
