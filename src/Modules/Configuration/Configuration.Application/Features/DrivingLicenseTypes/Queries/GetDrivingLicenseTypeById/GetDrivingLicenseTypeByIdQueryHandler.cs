using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Queries.GetDrivingLicenseTypeById;

/// <summary>Handles <see cref="GetDrivingLicenseTypeByIdQuery"/>.</summary>
public sealed class GetDrivingLicenseTypeByIdQueryHandler : IRequestHandler<GetDrivingLicenseTypeByIdQuery, Result<DrivingLicenseTypeDto>>
{
    private const string RequiredPermission = "DrivingLicenseType.View";

    private readonly IDrivingLicenseTypeRepository _drivingLicenseTypeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetDrivingLicenseTypeByIdQueryHandler"/> class.</summary>
    public GetDrivingLicenseTypeByIdQueryHandler(
        IDrivingLicenseTypeRepository drivingLicenseTypeRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _drivingLicenseTypeRepository = drivingLicenseTypeRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetDrivingLicenseTypeByIdQuery"/>.</summary>
    public async Task<Result<DrivingLicenseTypeDto>> Handle(GetDrivingLicenseTypeByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<DrivingLicenseTypeDto>(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        var entity = await _drivingLicenseTypeRepository.GetByIdAsync(
            global::Configuration.Domain.DrivingLicenseTypeId.From(request.DrivingLicenseTypeId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure<DrivingLicenseTypeDto>(Error.NotFound("DrivingLicenseType.NotFound", $"Driving License Type with id {request.DrivingLicenseTypeId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<DrivingLicenseTypeDto>(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        return Result.Success(entity.Adapt<DrivingLicenseTypeDto>());
    }
}