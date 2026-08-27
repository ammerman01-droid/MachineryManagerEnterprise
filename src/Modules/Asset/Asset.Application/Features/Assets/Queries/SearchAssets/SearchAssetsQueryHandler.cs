using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Queries.SearchAssets;

/// <summary>
/// Handles <see cref="SearchAssetsQuery"/> by verifying the caller is
/// authorized for the requested Organization, then delegating to the
/// repository search projection.
/// </summary>
public sealed class SearchAssetsQueryHandler
    : IRequestHandler<SearchAssetsQuery, Result<SearchAssetsResponse>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly IAssetRepository _assetRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="SearchAssetsQueryHandler"/> class.</summary>
    public SearchAssetsQueryHandler(
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

    /// <summary>Executes the search query.</summary>
    public async Task<Result<SearchAssetsResponse>> Handle(
        SearchAssetsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchAssetsResponse>(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(
            request.OrganizationId,
            cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<SearchAssetsResponse>(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var response = await _assetRepository.SearchAsync(
            request.OrganizationId,
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}
