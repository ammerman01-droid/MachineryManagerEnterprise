using MachineryManagerEnterprise.Asset.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;

/// <summary>
/// Handles <see cref="RegisterAssetModelCommand"/> by verifying the
/// Holding, Company, and every technical specification's Unit of
/// Measurement, invoking domain registration, persisting the
/// aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterAssetModelCommandHandler
    : IRequestHandler<RegisterAssetModelCommand, Result<Guid>>
{
    private const string RequiredPermission = "Asset.Create";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IUnitOfMeasurementLookupService _unitOfMeasurementLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterAssetModelCommandHandler"/> class.</summary>
    /// <param name="assetModelRepository">The Asset Model repository.</param>
    /// <param name="holdingLookupService">Cross-module lookup for Holding existence.</param>
    /// <param name="configurationLookupService">Cross-module lookup for Configuration-module master data (Company, in this handler).</param>
    /// <param name="unitOfMeasurementLookupService">Cross-module lookup for Unit of Measurement existence, Holding membership, and physical-quantity kind.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public RegisterAssetModelCommandHandler(
        IAssetModelRepository assetModelRepository,
        IHoldingLookupService holdingLookupService,
        IConfigurationLookupService configurationLookupService,
        IUnitOfMeasurementLookupService unitOfMeasurementLookupService,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _holdingLookupService = holdingLookupService;
        _configurationLookupService = configurationLookupService;
        _unitOfMeasurementLookupService = unitOfMeasurementLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new Asset Model's identifier, or a business error.</returns>
    public async Task<Result<Guid>> Handle(RegisterAssetModelCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(request.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var holdingExists = await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken);

        if (!holdingExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.HoldingNotFound(request.HoldingId));
        }

        var companyExists = await _configurationLookupService.CompanyExistsInHoldingAsync(
            request.CompanyId, request.HoldingId, cancellationToken);

        if (!companyExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.CompanyNotFound(request.CompanyId));
        }

        // Validate every provided technical specification's Unit of
        // Measurement exists, belongs to this Holding, AND belongs to
        // the physically correct category (chat, 2026-09-04 — mirrors
        // the equivalent check in RegisterEngineModelCommandHandler).
        var specifications = new (string FieldName, Guid? UnitOfMeasurementId, PhysicalQuantityKind ExpectedKind)[]
        {
            ("Length", request.LengthUnitOfMeasurementId, PhysicalQuantityKind.Dimension),
            ("Width", request.WidthUnitOfMeasurementId, PhysicalQuantityKind.Dimension),
            ("Height", request.HeightUnitOfMeasurementId, PhysicalQuantityKind.Dimension),
            ("Weight", request.WeightUnitOfMeasurementId, PhysicalQuantityKind.Weight),
            ("Working capacity (volume)", request.WorkingCapacityVolumeUnitOfMeasurementId, PhysicalQuantityKind.Dimension),
            ("Working capacity (weight)", request.WorkingCapacityWeightUnitOfMeasurementId, PhysicalQuantityKind.Weight),
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
                return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.UnitOfMeasurementNotFound(id));
            }

            var matchesKind = await _unitOfMeasurementLookupService.ExistsInHoldingWithKindAsync(
                id, request.HoldingId, expectedKind, cancellationToken);

            if (!matchesKind)
            {
                return Result.Failure<Guid>(
                    global::Asset.Domain.AssetModelErrors.UnitOfMeasurementKindMismatch(fieldName, expectedKind));
            }
        }

        var result = global::Asset.Domain.AssetModel.Register(
            request.HoldingId,
            request.Name,
            request.CompanyId,
            _dateTimeProvider,
            request.LengthValue,
            request.LengthUnitOfMeasurementId,
            request.WidthValue,
            request.WidthUnitOfMeasurementId,
            request.HeightValue,
            request.HeightUnitOfMeasurementId,
            request.WeightValue,
            request.WeightUnitOfMeasurementId,
            request.WorkingCapacityVolumeValue,
            request.WorkingCapacityVolumeUnitOfMeasurementId,
            request.WorkingCapacityWeightValue,
            request.WorkingCapacityWeightUnitOfMeasurementId);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _assetModelRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}