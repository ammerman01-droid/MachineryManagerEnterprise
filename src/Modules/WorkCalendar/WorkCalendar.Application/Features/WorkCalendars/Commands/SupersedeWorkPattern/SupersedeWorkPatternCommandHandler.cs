using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.SupersedeWorkPattern;

/// <summary>Handles <see cref="SupersedeWorkPatternCommand"/> by loading the aggregate, invoking domain supersession, and committing the unit of work.</summary>
public sealed class SupersedeWorkPatternCommandHandler : IRequestHandler<SupersedeWorkPatternCommand, Result<Guid>>
{
    private const string RequiredPermission = "WorkCalendar.Edit";

    private readonly IWorkCalendarRepository _repository;
    private readonly IProjectLookupService _projectLookupService;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="SupersedeWorkPatternCommandHandler"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public SupersedeWorkPatternCommandHandler(
        IWorkCalendarRepository repository,
        IProjectLookupService projectLookupService,
        IWorkCalendarUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _repository = repository;
        _projectLookupService = projectLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the use case.</summary>
    /// <param name="request">The command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new pattern version's identifier, or a business error.</returns>
    public async Task<Result<Guid>> Handle(SupersedeWorkPatternCommand request, CancellationToken cancellationToken)
    {
        var id = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(id, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure<Guid>(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        calendar.SynchronizeLifecycle(DateOnly.FromDateTime(_dateTimeProvider.UtcNow.DateTime));
        
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var patternId = global::WorkCalendar.Domain.WorkPatternId.From(request.WorkPatternId);

        var result = calendar.SupersedeWorkPattern(
            patternId, request.StartDate, request.EndDate, request.WeeklySchedule, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _repository.Update(calendar);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Value);
    }
}