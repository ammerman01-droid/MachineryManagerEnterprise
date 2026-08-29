using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.Assets.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Queries.GetAssetById;

/// <summary>
/// Handles <see cref="GetAssetByIdQuery"/> by loading the aggregate,
/// verifying the caller is authorized for its owning Organization, and
/// mapping it to a DTO.
/// </summary>
public sealed class GetAssetByIdQueryHandler
    : IRequestHandler<GetAssetByIdQuery, Result<AssetDto>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly IAssetRepository _assetRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetAssetByIdQueryHandler"/> class.</summary>
    public GetAssetByIdQueryHandler(
        IAssetRepository assetRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _assetRepository = assetRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the lookup use case.</summary>
    public async Task<Result<AssetDto>> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.AssetId.From(request.AssetId);
        var asset = await _assetRepository.GetByIdAsync(id, cancellationToken);

        if (asset is null)
        {
            return Result.Failure<AssetDto>(
                Error.NotFound("Asset.NotFound", $"Asset with id {request.AssetId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<AssetDto>(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(asset.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, asset.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<AssetDto>(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var dto = new AssetDto(
            asset.Id.Value,
            asset.OrganizationId,
            asset.Code,
            asset.AssetModelId.Value,
            asset.SerialNumber,
            asset.LicensePlate,
            asset.ManufactureYear,
            asset.Color,
            asset.Status.ToString());

        return Result.Success(dto);
    }
}