using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.SearchMeterDevices;

/// <summary>
/// Handles <see cref="SearchMeterDevicesQuery"/> by authorizing against the given Organization,
/// delegating to <see cref="IMeterDeviceRepository.SearchAsync"/>, and then enriching each row
/// with the owning Asset's code and name (chat, 2026-10-04) so the Presentation layer can show
/// something readable instead of a raw identifier.
/// </summary>
public sealed class SearchMeterDevicesQueryHandler
    : IRequestHandler<SearchMeterDevicesQuery, Result<SearchMeterDevicesResponse>>
{
    private const string RequiredPermission = "MeterDevice.View";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IAssetLookupService _assetLookupService;

    /// <summary>Initializes a new instance of the <see cref="SearchMeterDevicesQueryHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the Organization's Holding.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to resolve each owning Asset's code and name for display.</param>
    public SearchMeterDevicesQueryHandler(
        IMeterDeviceRepository meterDeviceRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService,
        IAssetLookupService assetLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
        _assetLookupService = assetLookupService;
    }

    /// <summary>Executes the device search.</summary>
    public async Task<Result<SearchMeterDevicesResponse>> Handle(SearchMeterDevicesQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchMeterDevicesResponse>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<SearchMeterDevicesResponse>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var response = await _meterDeviceRepository.SearchAsync(
            request.OrganizationId, request.Page, request.PageSize, cancellationToken);

        // One batched lookup for the whole page — not one call per row — so a page of
        // 200 devices costs a single extra query against the Asset module.
        var ownerAssetIds = response.Items
            .Where(item => item.OwnerType == "Asset" && item.OwnerId is not null)
            .Select(item => item.OwnerId!.Value)
            .Distinct()
            .ToList();

        if (ownerAssetIds.Count == 0)
        {
            return Result.Success(response);
        }

        var displayInfos = await _assetLookupService.GetDisplayInfosAsync(ownerAssetIds, cancellationToken);

        var enrichedItems = response.Items
            .Select(item => item.OwnerId is { } ownerId && displayInfos.TryGetValue(ownerId, out var info)
                ? item with { OwnerCode = info.Code, OwnerName = info.Name }
                : item)
            .ToList();

        return Result.Success(response with { Items = enrichedItems });
    }
}
