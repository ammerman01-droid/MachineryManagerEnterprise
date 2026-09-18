using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RegisterDrivingLicenseType;

/// <summary>Handles <see cref="RegisterDrivingLicenseTypeCommand"/>. Scope is resolved directly from HoldingId (same as Color).</summary>
public sealed class RegisterDrivingLicenseTypeCommandHandler : IRequestHandler<RegisterDrivingLicenseTypeCommand, Result<Guid>>
{
    private const string RequiredPermission = "DrivingLicenseType.Create";

    private readonly IDrivingLicenseTypeRepository _drivingLicenseTypeRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    // FILE: RegisterDrivingLicenseTypeCommandHandler.cs
// Modification type: Insert XML doc immediately above constructor and Handle method
    /// <summary>Initializes a new instance of the <see cref="RegisterDrivingLicenseTypeCommandHandler"/> class.</summary>
    public RegisterDrivingLicenseTypeCommandHandler(
        IDrivingLicenseTypeRepository drivingLicenseTypeRepository,
        IHoldingLookupService holdingLookupService,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _drivingLicenseTypeRepository = drivingLicenseTypeRepository;
        _holdingLookupService = holdingLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

        /// <summary>Handles <see cref="RegisterDrivingLicenseTypeCommand"/>.</summary>
    public async Task<Result<Guid>> Handle(RegisterDrivingLicenseTypeCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        if (!await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("Holding.NotFound", $"Holding with id {request.HoldingId} was not found."));
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.DrivingLicenseTypeErrors.NotAuthorized());
        }

        var result = global::Configuration.Domain.DrivingLicenseType.Register(request.HoldingId, request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _drivingLicenseTypeRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}