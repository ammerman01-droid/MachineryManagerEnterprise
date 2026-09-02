using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RegisterEngineModel;

/// <summary>
/// Handles <see cref="RegisterEngineModelCommand"/> by verifying the
/// Holding and Company, validating units, invoking domain registration,
/// persisting the aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterEngineModelCommandHandler
    : IRequestHandler<RegisterEngineModelCommand, Result<Guid>>
{
    private const string RequiredPermission = "Asset.Create";

    private readonly IEngineModelRepository _engineModelRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IUnitOfMeasurementLookupService _unitOfMeasurementLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterEngineModelCommandHandler"/> class.</summary>
    public RegisterEngineModelCommandHandler(
        IEngineModelRepository engineModelRepository,
        IHoldingLookupService holdingLookupService,
        IUnitOfMeasurementLookupService unitOfMeasurementLookupService,
        IConfigurationLookupService configurationLookupService,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _engineModelRepository = engineModelRepository;
        _holdingLookupService = holdingLookupService;
        _unitOfMeasurementLookupService = unitOfMeasurementLookupService;
        _configurationLookupService = configurationLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterEngineModelCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(request.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var holdingExists = await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken);

        if (!holdingExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.EngineModelErrors.HoldingNotFound(request.HoldingId));
        }

        var companyExists = await _configurationLookupService.CompanyExistsInHoldingAsync(
            request.CompanyId, request.HoldingId, cancellationToken);

        if (!companyExists)
        {
            return Result.Failure<Guid>(
                global::Asset.Domain.EngineModelErrors.CompanyNotFound(request.CompanyId));
        }

        var specifications = new (string FieldName, Guid? UnitOfMeasurementId, PhysicalQuantityKind ExpectedKind)[]
        {
            ("Engine displacement", request.EngineDisplacementUnitOfMeasurementId, PhysicalQuantityKind.Dimension),
            ("Engine power", request.EnginePowerUnitOfMeasurementId, PhysicalQuantityKind.Force),
            ("Weight", request.WeightUnitOfMeasurementId, PhysicalQuantityKind.Weight),
        };

        foreach (var (fieldName, unitOfMeasurementId, expectedKind) in specifications)
        {
            if (unitOfMeasurementId is not { } id)
            {
                continue;
            }

            var existsInHolding = await _unitOfMeasurementLookupService.ExistsInHoldingAsync(
                id, request.HoldingId, cancellationToken);

            if (!existsInHolding)
            {
                return Result.Failure<Guid>(global::Asset.Domain.EngineModelErrors.UnitOfMeasurementNotFound(id));
            }

            var matchesKind = await _unitOfMeasurementLookupService.ExistsInHoldingWithKindAsync(
                id, request.HoldingId, expectedKind, cancellationToken);

            if (!matchesKind)
            {
                return Result.Failure<Guid>(
                    global::Asset.Domain.EngineModelErrors.UnitOfMeasurementKindMismatch(fieldName, expectedKind));
            }
        }

        var result = global::Asset.Domain.EngineModel.Register(
            request.HoldingId,
            request.Name,
            request.CompanyId,
            _dateTimeProvider,
            request.CylinderCount,
            request.EngineDisplacementValue,
            request.EngineDisplacementUnitOfMeasurementId,
            request.EnginePowerValue,
            request.EnginePowerUnitOfMeasurementId,
            request.WeightValue,
            request.WeightUnitOfMeasurementId);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _engineModelRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}