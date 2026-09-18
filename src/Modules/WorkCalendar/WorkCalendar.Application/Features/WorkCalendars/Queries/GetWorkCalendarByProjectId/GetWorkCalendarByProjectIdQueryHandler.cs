using Mapster;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;
using MapsterMapper;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarByProjectId;

/// <summary>
/// Handles <see cref="GetWorkCalendarByProjectIdQuery"/> by loading the
/// aggregate via <see cref="IWorkCalendarRepository.GetByProjectIdAsync"/>,
/// verifying the caller is authorized for the Project, and mapping it to
/// a DTO. Mirrors <c>GetWorkCalendarByIdQueryHandler</c> exactly, only
/// the lookup key differs.
/// </summary>
public sealed class GetWorkCalendarByProjectIdQueryHandler
    : IRequestHandler<GetWorkCalendarByProjectIdQuery, Result<WorkCalendarDto>>
{
    private const string RequiredPermission = "WorkCalendar.View";

    private readonly IWorkCalendarRepository _workCalendarRepository;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetWorkCalendarByProjectIdQueryHandler"/> class.</summary>
    /// <param name="workCalendarRepository">The Work Calendar repository.</param>
    /// <param name="projectLookupService">Cross-module lookup for Project existence and its Organization/Holding ownership chain.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the aggregate to a DTO.</param>
    public GetWorkCalendarByProjectIdQueryHandler(
        IWorkCalendarRepository workCalendarRepository,
        IProjectLookupService projectLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _workCalendarRepository = workCalendarRepository;
        _projectLookupService = projectLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <summary>Executes the lookup use case.</summary>
    /// <param name="request">The query containing the Project identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="WorkCalendarDto"/>, or a business error.</returns>
    public async Task<Result<WorkCalendarDto>> Handle(GetWorkCalendarByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var workCalendar = await _workCalendarRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        if (workCalendar is null)
        {
            return Result.Failure<WorkCalendarDto>(
                Error.NotFound("WorkCalendar.NotFoundForProject", $"Project with id {request.ProjectId} has no Work Calendar yet."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<WorkCalendarDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(request.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(request.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, organizationId, request.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<WorkCalendarDto>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        return Result.Success(_mapper.Map<WorkCalendarDto>(workCalendar));
    }
}
