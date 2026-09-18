using Mapster;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarById;

/// <summary>
/// Handles <see cref="GetWorkCalendarByIdQuery"/> by loading the
/// aggregate, verifying the caller is authorized for its Project, and
/// mapping it to a DTO via Mapster.
/// </summary>
public sealed class GetWorkCalendarByIdQueryHandler
    : IRequestHandler<GetWorkCalendarByIdQuery, Result<WorkCalendarDto>>
{
    private const string RequiredPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _workCalendarRepository;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetWorkCalendarByIdQueryHandler"/> class.</summary>
    /// <param name="workCalendarRepository">The Work Calendar repository.</param>
    /// <param name="unitOfWork">The unit of work, used to persist any lifecycle auto-correction.</param>
    /// <param name="projectLookupService">Cross-module lookup for Project existence and its Organization/Holding ownership chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the aggregate to a DTO.</param>
    public GetWorkCalendarByIdQueryHandler(
        IWorkCalendarRepository workCalendarRepository,
        IWorkCalendarUnitOfWork unitOfWork,
        IProjectLookupService projectLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _workCalendarRepository = workCalendarRepository;
        _unitOfWork = unitOfWork;
        _projectLookupService = projectLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    /// <summary>Executes the lookup use case.</summary>
    /// <param name="request">The query containing the Work Calendar identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="WorkCalendarDto"/>, or a business error.</returns>
    public async Task<Result<WorkCalendarDto>> Handle(GetWorkCalendarByIdQuery request, CancellationToken cancellationToken)
    {
        var id = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var workCalendar = await _workCalendarRepository.GetByIdAsync(id, cancellationToken);

        if (workCalendar is null)
        {
            return Result.Failure<WorkCalendarDto>(
                Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        if (workCalendar.SynchronizeLifecycle(DateOnly.FromDateTime(_dateTimeProvider.UtcNow.DateTime)))
        {
            _workCalendarRepository.Update(workCalendar);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<WorkCalendarDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(workCalendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(workCalendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, organizationId, workCalendar.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<WorkCalendarDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var dto = _mapper.Map<WorkCalendarDto>(workCalendar);

        return Result.Success(dto);
    }
}