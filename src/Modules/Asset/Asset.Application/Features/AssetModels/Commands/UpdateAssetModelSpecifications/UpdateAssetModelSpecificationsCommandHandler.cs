using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.UpdateAssetModelSpecifications;

/// <summary>
/// Handles <see cref="UpdateAssetModelSpecificationsCommand"/> by
/// loading the aggregate, re-validating the Company and every Unit of
/// Measurement against its Holding, invoking the domain update, and
/// committing the unit of work.
/// </summary>
public sealed class UpdateAssetModelSpecificationsCommandHandler
    : IRequestHandler<UpdateAssetModelSpecificationsCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IUnitOfMeasurementLookupService _unitOfMeasurementLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="UpdateAssetModelSpecificationsCommandHandler"/> class.</summary>
    /// <param name="assetModelRepository">The Asset Model repository.</param>
    /// <param name="configurationLookupService">Cross-module lookup for Configuration-module master data (Company, in this handler).</param>
    /// <param name="unitOfMeasurementLookupService">Cross-module lookup for Unit of Measurement existence, Holding membership, and physical-quantity kind.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public UpdateAssetModelSpecificationsCommandHandler(
        IAssetModelRepository assetModelRepository,
        IConfigurationLookupService configurationLookupService,
        IUnitOfMeasurementLookupService unitOfMeasurementLookupService,
        IAssetUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _configurationLookupService = configurationLookupService;
        _unitOfMeasurementLookupService = unitOfMeasurementLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the update use case.</summary>
    /// <param name="request">The update command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(UpdateAssetModelSpecificationsCommand request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.AssetModelId.From(request.AssetModelId);
        var assetModel = await _assetModelRepository.GetByIdAsync(id, cancellationToken);

        if (assetModel is null)
        {
            return Result.Failure(
                Error.NotFound("AssetModel.NotFound", $"Asset model with id {request.AssetModelId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(assetModel.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var companyExists = await _configurationLookupService.CompanyExistsInHoldingAsync(
            request.CompanyId, assetModel.HoldingId, cancellationToken);

        if (!companyExists)
        {
            return Result.Failure(global::Asset.Domain.AssetModelErrors.CompanyNotFound(request.CompanyId));
        }

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
            if (unitOfMeasurementId is not { } uomId)
            {
                continue;
            }

            var existsInHolding = await _unitOfMeasurementLookupService.ExistsInHoldingAsync(
                uomId, assetModel.HoldingId, cancellationToken);

            if (!existsInHolding)
            {
                return Result.Failure(global::Asset.Domain.AssetModelErrors.UnitOfMeasurementNotFound(uomId));
            }

            var matchesKind = await _unitOfMeasurementLookupService.ExistsInHoldingWithKindAsync(
                uomId, assetModel.HoldingId, expectedKind, cancellationToken);

            if (!matchesKind)
            {
                return Result.Failure(
                    global::Asset.Domain.AssetModelErrors.UnitOfMeasurementKindMismatch(fieldName, expectedKind));
            }
        }

        var result = assetModel.UpdateSpecifications(
            request.CompanyId,
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
            return result;
        }

        _assetModelRepository.Update(assetModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}