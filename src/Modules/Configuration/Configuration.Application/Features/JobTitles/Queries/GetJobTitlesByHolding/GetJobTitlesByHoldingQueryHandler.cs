using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Queries.GetJobTitlesByHolding;

/// <summary>Handles <see cref="GetJobTitlesByHoldingQuery"/>.</summary>
public sealed class GetJobTitlesByHoldingQueryHandler : IRequestHandler<GetJobTitlesByHoldingQuery, Result<IReadOnlyList<JobTitleDto>>>
{
    private const string RequiredPermission = "JobTitle.View";

    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetJobTitlesByHoldingQueryHandler"/> class.</summary>
    public GetJobTitlesByHoldingQueryHandler(
        IJobTitleRepository jobTitleRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _jobTitleRepository = jobTitleRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetJobTitlesByHoldingQuery"/>.</summary>
    public async Task<Result<IReadOnlyList<JobTitleDto>>> Handle(GetJobTitlesByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<JobTitleDto>>(global::Configuration.Domain.JobTitleErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<JobTitleDto>>(global::Configuration.Domain.JobTitleErrors.NotAuthorized());
        }

        var jobTitles = await _jobTitleRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(jobTitles);
    }
}