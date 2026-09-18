using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.DeleteDrivingLicenseType;

/// <summary>
/// Handles <see cref="DeleteDrivingLicenseTypeCommand"/>. Deletion is
/// blocked (Conflict) if any Personnel record currently references this
/// Driving License Type, checked via the cross-module
/// <see cref="IPersonnelUsageLookupService"/>.
/// </summary>
public sealed class DeleteDrivingLicenseTypeCommandHandler : IRequestHandler<DeleteDrivingLicenseTypeCommand, Result>
{
    private const string RequiredPermission = "DrivingLicenseType.Delete";

    private readonly IDrivingLicenseTypeRepository _drivingLicenseTypeRepository;
    private readonly IPersonnelUsageLookupService _personnelUsageLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeleteDrivingLicenseTypeCommandHandler"/> class.</summary>
    public DeleteDrivingLicenseTypeCommandHandler(
        IDrivingLicenseTypeRepository drivingLicenseTypeRepository,
        IPersonnelUsageLookupService personnelUsageLookupService,
        IConfigurationUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _drivingLicenseTypeRepository = drivingLicenseTypeRepository;
        _personnelUsageLookupService = personnelUsageLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="DeleteDrivingLicenseTypeCommand"/>.</summary>
    public async Task<Result> Handle(DeleteDrivingLicenseTypeCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        var entity = await _drivingLicenseTypeRepository.GetByIdAsync(
            global::Configuration.Domain.DrivingLicenseTypeId.From(request.DrivingLicenseTypeId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure(Error.NotFound("DrivingLicenseType.NotFound", $"Driving License Type with id {request.DrivingLicenseTypeId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        if (await _personnelUsageLookupService.IsDrivingLicenseTypeInUseAsync(request.DrivingLicenseTypeId, cancellationToken))
        {
            return Result.Failure(global::Configuration.Domain.DrivingLicenseTypeErrors.InUse());
        }

        _drivingLicenseTypeRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}