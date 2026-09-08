using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RemoveCompatibleEngineModel;

/// <summary>
/// Handles <see cref="RemoveCompatibleEngineModelCommand"/> by loading
/// the asset model, invoking the domain compatibility-removal
/// behavior, and committing the unit of work.
/// </summary>
public sealed class RemoveCompatibleEngineModelCommandHandler
    : IRequestHandler<RemoveCompatibleEngineModelCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RemoveCompatibleEngineModelCommandHandler"/> class.</summary>
    public RemoveCompatibleEngineModelCommandHandler(
        IAssetModelRepository assetModelRepository,
        IAssetUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the compatibility-removal use case.</summary>
    public async Task<Result> Handle(RemoveCompatibleEngineModelCommand request, CancellationToken cancellationToken)
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
        var result = assetModel.RemoveCompatibleEngineModel(engineModelId);

        if (result.IsFailure)
        {
            return result;
        }

        _assetModelRepository.Update(assetModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}