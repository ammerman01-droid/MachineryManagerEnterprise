using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.AssignCompatibleEngineModel;

/// <summary>
/// Handles <see cref="AssignCompatibleEngineModelCommand"/> by loading
/// the asset model, invoking the domain compatibility behavior, and
/// committing the unit of work.
/// </summary>
public sealed class AssignCompatibleEngineModelCommandHandler
    : IRequestHandler<AssignCompatibleEngineModelCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="AssignCompatibleEngineModelCommandHandler"/> class.</summary>
    public AssignCompatibleEngineModelCommandHandler(
        IAssetModelRepository assetModelRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _assetModelRepository = assetModelRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
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

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(assetModel.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, assetModel.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var engineModelId = global::Asset.Domain.EngineModelId.From(request.EngineModelId);
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