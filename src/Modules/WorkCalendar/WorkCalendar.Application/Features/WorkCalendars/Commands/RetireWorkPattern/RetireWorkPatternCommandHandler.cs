using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RetireWorkPattern;

/// <summary>Handles <see cref="RetireWorkPatternCommand"/>.</summary>
public sealed class RetireWorkPatternCommandHandler : IRequestHandler<RetireWorkPatternCommand, Result>
{
    // Matches the "Delete"-permission precedent set by ArchiveWorkCalendar
    // (a permanent, one-way deactivation) rather than "Edit".
    private const string RequiredPermission = "WorkCalendar.Delete";

    private readonly IWorkCalendarRepository _repository;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly IProjectLookupService _projectLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>Initializes a new instance of the <see cref="RetireWorkPatternCommandHandler"/> class.</summary>
    public RetireWorkPatternCommandHandler(
        IWorkCalendarRepository repository,
        IWorkCalendarUnitOfWork unitOfWork,
        IProjectLookupService projectLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IDateTimeProvider dateTimeProvider)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _projectLookupService = projectLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>Executes the retirement use case.</summary>
    /// <param name="request">The command containing the calendar and pattern identifiers.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success, or a business error.</returns>
    public async Task<Result> Handle(RetireWorkPatternCommand request, CancellationToken cancellationToken)
    {
        var id = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(id, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

        calendar.SynchronizeLifecycle(DateOnly.FromDateTime(_dateTimeProvider.UtcNow.DateTime));

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(calendar.ProjectId, cancellationToken);
        var holdingId = await _projectLookupService.GetHoldingIdAsync(calendar.ProjectId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(holdingId, organizationId, calendar.ProjectId), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var result = calendar.RetireWorkPattern(
            global::WorkCalendar.Domain.WorkPatternId.From(request.WorkPatternId), _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}