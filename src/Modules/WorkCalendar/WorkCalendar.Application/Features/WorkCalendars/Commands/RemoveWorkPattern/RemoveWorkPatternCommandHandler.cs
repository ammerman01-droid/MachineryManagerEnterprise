using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RemoveWorkPattern;

/// <summary>Handles <see cref="RemoveWorkPatternCommand"/> by loading the aggregate, invoking domain removal, and committing the unit of work.</summary>
public sealed class RemoveWorkPatternCommandHandler : IRequestHandler<RemoveWorkPatternCommand, Result>
{
    private const string RequiredPermission = "WorkCalendar.Delete";

    private readonly IWorkCalendarRepository _repository;
    private readonly IProjectLookupService _projectLookupService;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RemoveWorkPatternCommandHandler"/> class.</summary>
    /// <param name="repository">The Work Calendar repository.</param>
    /// <param name="projectLookupService">Cross-module lookup for the governing Project's Organization/Holding chain.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public RemoveWorkPatternCommandHandler(
        IWorkCalendarRepository repository,
        IProjectLookupService projectLookupService,
        IWorkCalendarUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _repository = repository;
        _projectLookupService = projectLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the use case.</summary>
    /// <param name="request">The command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(RemoveWorkPatternCommand request, CancellationToken cancellationToken)
    {
        var id = global::WorkCalendar.Domain.WorkCalendarId.From(request.WorkCalendarId);
        var calendar = await _repository.GetByIdAsync(id, cancellationToken);

        if (calendar is null)
        {
            return Result.Failure(Error.NotFound("WorkCalendar.NotFound", $"Work calendar with id {request.WorkCalendarId} was not found."));
        }

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

        var patternId = global::WorkCalendar.Domain.WorkPatternId.From(request.WorkPatternId);
        var result = calendar.RemoveWorkPattern(patternId);

        if (result.IsFailure)
        {
            return result;
        }

        _repository.Update(calendar);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}