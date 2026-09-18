using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.DeleteJobTitle;

/// <summary>
/// Handles <see cref="DeleteJobTitleCommand"/>. Deletion is blocked
/// (Conflict) if any Personnel record currently references this Job
/// Title, checked via the cross-module <see cref="IPersonnelUsageLookupService"/>.
/// </summary>
public sealed class DeleteJobTitleCommandHandler : IRequestHandler<DeleteJobTitleCommand, Result>
{
    private const string RequiredPermission = "JobTitle.Delete";

    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly IPersonnelUsageLookupService _personnelUsageLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeleteJobTitleCommandHandler"/> class.</summary>
    public DeleteJobTitleCommandHandler(
        IJobTitleRepository jobTitleRepository,
        IPersonnelUsageLookupService personnelUsageLookupService,
        IConfigurationUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _jobTitleRepository = jobTitleRepository;
        _personnelUsageLookupService = personnelUsageLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="DeleteJobTitleCommand"/>.</summary>
    public async Task<Result> Handle(DeleteJobTitleCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Configuration.Domain.JobTitleErrors.NotAuthorized());
        }

        var entity = await _jobTitleRepository.GetByIdAsync(
            global::Configuration.Domain.JobTitleId.From(request.JobTitleId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure(Error.NotFound("JobTitle.NotFound", $"Job Title with id {request.JobTitleId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Configuration.Domain.JobTitleErrors.NotAuthorized());
        }

        if (await _personnelUsageLookupService.IsJobTitleInUseAsync(request.JobTitleId, cancellationToken))
        {
            return Result.Failure(global::Configuration.Domain.JobTitleErrors.InUse());
        }

        _jobTitleRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}