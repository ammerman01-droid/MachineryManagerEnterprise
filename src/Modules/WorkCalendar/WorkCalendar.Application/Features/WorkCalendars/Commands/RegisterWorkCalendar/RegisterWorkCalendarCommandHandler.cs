using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RegisterWorkCalendar;

/// <summary>
/// Handles <see cref="RegisterWorkCalendarCommand"/> by verifying the
/// referenced Project exists, invoking domain registration, persisting
/// the aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterWorkCalendarCommandHandler
    : IRequestHandler<RegisterWorkCalendarCommand, Result<Guid>>
{
    private const string RequiredPermission = "WorkCalendar.Create";

    private readonly IWorkCalendarRepository _workCalendarRepository;
    private readonly IProjectLookupService _projectLookupService;
    private readonly IWorkCalendarUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterWorkCalendarCommandHandler"/> class.</summary>
    /// <param name="workCalendarRepository">The Work Calendar repository.</param>
    /// <param name="projectLookupService">Cross-module lookup for Project existence and its Organization/Holding ownership chain.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public RegisterWorkCalendarCommandHandler(
        IWorkCalendarRepository workCalendarRepository,
        IProjectLookupService projectLookupService,
        IWorkCalendarUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _workCalendarRepository = workCalendarRepository;
        _projectLookupService = projectLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new Work Calendar's identifier, or a business error.</returns>
    public async Task<Result<Guid>> Handle(RegisterWorkCalendarCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var projectExists = await _projectLookupService.ExistsAsync(request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            return Result.Failure<Guid>(global::WorkCalendar.Domain.WorkCalendarErrors.ProjectNotFound(request.ProjectId));
        }

        var organizationId = await _projectLookupService.GetOrganizationIdAsync(request.ProjectId, cancellationToken);
        var holdingId = organizationId is { } orgId
            ? await _projectLookupService.GetHoldingIdAsync(request.ProjectId, cancellationToken)
            : null;

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, organizationId, request.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::WorkCalendar.Domain.WorkCalendarErrors.NotAuthorized());
        }

        var result = global::WorkCalendar.Domain.WorkCalendar.Register(request.ProjectId, request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _workCalendarRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}