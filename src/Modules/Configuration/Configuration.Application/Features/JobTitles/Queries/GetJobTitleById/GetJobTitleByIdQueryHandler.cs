using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Queries.GetJobTitleById;

/// <summary>Handles <see cref="GetJobTitleByIdQuery"/>.</summary>
public sealed class GetJobTitleByIdQueryHandler : IRequestHandler<GetJobTitleByIdQuery, Result<JobTitleDto>>
{
    private const string RequiredPermission = "JobTitle.View";

    private readonly IJobTitleRepository _JobTitleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetJobTitleByIdQueryHandler"/> class.</summary>
    public GetJobTitleByIdQueryHandler(
        IJobTitleRepository JobTitleRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _JobTitleRepository = JobTitleRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetJobTitleByIdQuery"/>.</summary>
    public async Task<Result<JobTitleDto>> Handle(GetJobTitleByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<JobTitleDto>(global::Configuration.Domain.JobTitleErrors.NotAuthorized());
        }

        var entity = await _JobTitleRepository.GetByIdAsync(
            global::Configuration.Domain.JobTitleId.From(request.JobTitleId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure<JobTitleDto>(Error.NotFound("JobTitle.NotFound", $"Driving License Type with id {request.JobTitleId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<JobTitleDto>(global::Configuration.Domain.JobTitleErrors.NotAuthorized());
        }

        return Result.Success(entity.Adapt<JobTitleDto>());
    }
}