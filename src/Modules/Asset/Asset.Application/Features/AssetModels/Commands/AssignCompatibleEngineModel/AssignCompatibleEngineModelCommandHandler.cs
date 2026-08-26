using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.AssignCompatibleEngineModel;

/// <summary>
/// Handles <see cref="AssignCompatibleEngineModelCommand"/> by loading
/// the asset model AND the referenced engine model, verifying both
/// exist and belong to the same Holding (chat, 2026-08-26 — gap fix:
/// AssetModel/EngineModel catalogs are Per-Holding, so a compatibility
/// link can never cross that boundary), invoking the domain
/// compatibility behavior, and committing the unit of work.
/// </summary>
public sealed class AssignCompatibleEngineModelCommandHandler
    : IRequestHandler<AssignCompatibleEngineModelCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IEngineModelRepository _engineModelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="AssignCompatibleEngineModelCommandHandler"/> class.</summary>
    public AssignCompatibleEngineModelCommandHandler(
        IAssetModelRepository assetModelRepository,
        IEngineModelRepository engineModelRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _engineModelRepository = engineModelRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the compatibility-assignment use case.</summary>
    public async Task<Result> Handle(AssignCompatibleEngineModelCommand request, CancellationToken cancellationToken)
    {
        var assetModelId = global::Asset.Domain.AssetModelId.From(request.AssetModelId);
        var assetModel = await _assetModelRepository.GetByIdAsync(assetModelId, cancellationToken);

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

        var engineModelId = global::Asset.Domain.EngineModelId.From(request.EngineModelId);
        var engineModel = await _engineModelRepository.GetByIdAsync(engineModelId, cancellationToken);

        if (engineModel is null)
        {
            return Result.Failure(
                global::Asset.Domain.AssetModelErrors.EngineModelNotFound(request.EngineModelId));
        }

        // AssetModel and EngineModel catalogs are Per-Holding (chat,
        // 2026-08-26); a compatibility link may never cross Holdings.
        if (engineModel.HoldingId != assetModel.HoldingId)
        {
            return Result.Failure(
                global::Asset.Domain.AssetModelErrors.EngineModelBelongsToDifferentHolding());
        }

        var result = assetModel.AssignCompatibleEngineModel(engineModelId, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _assetModelRepository.Update(assetModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
