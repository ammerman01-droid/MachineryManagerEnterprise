using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Queries.GetDrivingLicenseTypesByHolding;

/// <summary>Handles <see cref="GetDrivingLicenseTypesByHoldingQuery"/>.</summary>
public sealed class GetDrivingLicenseTypesByHoldingQueryHandler
    : IRequestHandler<GetDrivingLicenseTypesByHoldingQuery, Result<IReadOnlyList<DrivingLicenseTypeDto>>>
{
    private const string RequiredPermission = "DrivingLicenseType.View";

    private readonly IDrivingLicenseTypeRepository _drivingLicenseTypeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetDrivingLicenseTypesByHoldingQueryHandler"/> class.</summary>
    public GetDrivingLicenseTypesByHoldingQueryHandler(
        IDrivingLicenseTypeRepository drivingLicenseTypeRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _drivingLicenseTypeRepository = drivingLicenseTypeRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

        /// <summary>Handles <see cref="GetDrivingLicenseTypesByHoldingQuery"/>.</summary>
    public async Task<Result<IReadOnlyList<DrivingLicenseTypeDto>>> Handle(GetDrivingLicenseTypesByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<DrivingLicenseTypeDto>>(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<DrivingLicenseTypeDto>>(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        var types = await _drivingLicenseTypeRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(types);
    }
}