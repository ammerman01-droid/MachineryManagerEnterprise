using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.DeactivateAsset;

/// <summary>
/// Handles <see cref="DeactivateAssetCommand"/> by loading the
/// aggregate, invoking the domain deactivation behavior, and
/// committing the unit of work.
/// </summary>
public sealed class DeactivateAssetCommandHandler
    : IRequestHandler<DeactivateAssetCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IAssetRepository _assetRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="DeactivateAssetCommandHandler"/> class.</summary>
    public DeactivateAssetCommandHandler(
        IAssetRepository assetRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _assetRepository = assetRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the deactivation use case.</summary>
    public async Task<Result> Handle(DeactivateAssetCommand request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.AssetId.From(request.AssetId);
        var asset = await _assetRepository.GetByIdAsync(id, cancellationToken);

        if (asset is null)
        {
            return Result.Failure(
                Error.NotFound("Asset.NotFound", $"Asset with id {request.AssetId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(asset.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, asset.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var result = asset.Deactivate(_dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _assetRepository.Update(asset);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}