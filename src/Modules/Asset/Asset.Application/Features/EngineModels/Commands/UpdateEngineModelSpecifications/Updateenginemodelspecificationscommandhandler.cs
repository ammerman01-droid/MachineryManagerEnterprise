using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.UpdateEngineModelSpecifications;

/// <summary>
/// Handles <see cref="UpdateEngineModelSpecificationsCommand"/> by
/// loading the aggregate, authorizing the caller, validating units,
/// applying the domain update, and committing the unit of work.
/// </summary>
public sealed class UpdateEngineModelSpecificationsCommandHandler
    : IRequestHandler<UpdateEngineModelSpecificationsCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IEngineModelRepository _engineModelRepository;
    private readonly IUnitOfMeasurementLookupService _unitOfMeasurementLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="UpdateEngineModelSpecificationsCommandHandler"/> class.</summary>
    public UpdateEngineModelSpecificationsCommandHandler(
        IEngineModelRepository engineModelRepository,
        IUnitOfMeasurementLookupService unitOfMeasurementLookupService,
        IAssetUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _engineModelRepository = engineModelRepository;
        _unitOfMeasurementLookupService = unitOfMeasurementLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the update use case.</summary>
    public async Task<Result> Handle(UpdateEngineModelSpecificationsCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var id = global::Asset.Domain.EngineModelId.From(request.EngineModelId);
        var engineModel = await _engineModelRepository.GetByIdAsync(id, cancellationToken);

        if (engineModel is null)
        {
            return Result.Failure(
                Error.NotFound("EngineModel.NotFound", $"Engine model with id {request.EngineModelId} was not found."));
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(engineModel.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var specifications = new (string FieldName, Guid? UnitOfMeasurementId, PhysicalQuantityKind ExpectedKind)[]
        {
            ("Engine displacement", request.EngineDisplacementUnitOfMeasurementId, PhysicalQuantityKind.Dimension),
            ("Engine power", request.EnginePowerUnitOfMeasurementId, PhysicalQuantityKind.Force),
            ("Weight", request.WeightUnitOfMeasurementId, PhysicalQuantityKind.Weight),
        };

        foreach (var (fieldName, unitOfMeasurementId, expectedKind) in specifications)
        {
            if (unitOfMeasurementId is not { } uid)
            {
                continue;
            }

            var existsInHolding = await _unitOfMeasurementLookupService.ExistsInHoldingAsync(
                uid, engineModel.HoldingId, cancellationToken);

            if (!existsInHolding)
            {
                return Result.Failure(global::Asset.Domain.EngineModelErrors.UnitOfMeasurementNotFound(uid));
            }

            var matchesKind = await _unitOfMeasurementLookupService.ExistsInHoldingWithKindAsync(
                uid, engineModel.HoldingId, expectedKind, cancellationToken);

            if (!matchesKind)
            {
                return Result.Failure(
                    global::Asset.Domain.EngineModelErrors.UnitOfMeasurementKindMismatch(fieldName, expectedKind));
            }
        }

        var updateResult = engineModel.UpdateSpecifications(
            request.CompanyId,
            request.FuelKind,
            request.CylinderCount,
            request.EngineDisplacementValue,
            request.EngineDisplacementUnitOfMeasurementId,
            request.EnginePowerValue,
            request.EnginePowerUnitOfMeasurementId,
            request.WeightValue,
            request.WeightUnitOfMeasurementId);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        _engineModelRepository.Update(engineModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}