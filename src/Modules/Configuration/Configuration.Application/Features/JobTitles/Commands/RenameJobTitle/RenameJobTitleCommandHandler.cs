using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.RenameJobTitle;

/// <summary>Handles <see cref="RenameJobTitleCommand"/>.</summary>
public sealed class RenameJobTitleCommandHandler : IRequestHandler<RenameJobTitleCommand, Result>
{
    private const string RequiredPermission = "JobTitle.Edit";

    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RenameJobTitleCommandHandler"/> class.</summary>
    public RenameJobTitleCommandHandler(
        IJobTitleRepository jobTitleRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _jobTitleRepository = jobTitleRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="RenameJobTitleCommand"/>.</summary>
    public async Task<Result> Handle(RenameJobTitleCommand request, CancellationToken cancellationToken)
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

        var renameResult = entity.Rename(request.Name, _dateTimeProvider);

        if (renameResult.IsFailure)
        {
            return renameResult;
        }

        _jobTitleRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}