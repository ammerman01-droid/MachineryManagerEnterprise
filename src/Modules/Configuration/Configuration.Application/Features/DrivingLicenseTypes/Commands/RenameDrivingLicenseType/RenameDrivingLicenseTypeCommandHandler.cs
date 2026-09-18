using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RenameDrivingLicenseType;

/// <summary>Handles <see cref="RenameDrivingLicenseTypeCommand"/>.</summary>
public sealed class RenameDrivingLicenseTypeCommandHandler : IRequestHandler<RenameDrivingLicenseTypeCommand, Result>
{
    private const string RequiredPermission = "DrivingLicenseType.Edit";

    private readonly IDrivingLicenseTypeRepository _drivingLicenseTypeRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RenameDrivingLicenseTypeCommandHandler"/> class.</summary>
    public RenameDrivingLicenseTypeCommandHandler(
        IDrivingLicenseTypeRepository drivingLicenseTypeRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _drivingLicenseTypeRepository = drivingLicenseTypeRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="RenameDrivingLicenseTypeCommand"/>.</summary>
    public async Task<Result> Handle(RenameDrivingLicenseTypeCommand request, CancellationToken cancellationToken)
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

        var renameResult = entity.Rename(request.Name, _dateTimeProvider);

        if (renameResult.IsFailure)
        {
            return renameResult;
        }

        _drivingLicenseTypeRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}