using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListOrganizationUsageEntries;

/// <summary>
/// Handles <see cref="ListOrganizationUsageEntriesQuery"/> by checking permission, delegating to
/// <see cref="IUsageReadService.ListByOrganizationAsync"/>, and then enriching each row with the
/// owning Asset's code and the Project's name (chat, 2026-10-12) so the list shows readable values
/// instead of identifiers.
/// </summary>
public sealed class ListOrganizationUsageEntriesQueryHandler
    : IRequestHandler<ListOrganizationUsageEntriesQuery, Result<PagedResult<UsageEntryListItem>>>
{
    private const string RequiredPermission = "DailyUsage.View";

    private readonly IUsageReadService _usageReadService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IProjectLookupService _projectLookupService;

    /// <summary>Initializes a new instance of the <see cref="ListOrganizationUsageEntriesQueryHandler"/> class.</summary>
    /// <param name="usageReadService">The Usage module's read-only projection service.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the Organization's Holding.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to resolve each owning Asset's code for display.</param>
    /// <param name="projectLookupService">Cross-module, read-only lookup into the Organization module, used to resolve each Project's name for display.</param>
    public ListOrganizationUsageEntriesQueryHandler(
        IUsageReadService usageReadService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService,
        IAssetLookupService assetLookupService,
        IProjectLookupService projectLookupService)
    {
        _usageReadService = usageReadService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
        _assetLookupService = assetLookupService;
        _projectLookupService = projectLookupService;
    }

    /// <summary>Executes the organization-wide usage entries listing.</summary>
    public async Task<Result<PagedResult<UsageEntryListItem>>> Handle(ListOrganizationUsageEntriesQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<PagedResult<UsageEntryListItem>>(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<PagedResult<UsageEntryListItem>>(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var page = await _usageReadService.ListByOrganizationAsync(
            request.OrganizationId,
            request.FromDate,
            request.ToDate,
            request.ProjectId,
            request.MeterDeviceId,
            request.Page,
            request.PageSize,
            cancellationToken);

        if (page.Items.Count == 0)
        {
            return page;
        }

        // One batched Asset lookup for the whole page, and one name lookup per DISTINCT Project
        // (a page normally spans only a handful of Projects), instead of one call per row.
        var ownerAssetIds = page.Items
            .Where(item => item.OwnerType == global::Usage.Domain.UsageOwnerType.Asset)
            .Select(item => item.OwnerId)
            .Distinct()
            .ToList();

        IReadOnlyDictionary<Guid, AssetDisplayInfo> assetInfos = ownerAssetIds.Count == 0
            ? new Dictionary<Guid, AssetDisplayInfo>()
            : await _assetLookupService.GetDisplayInfosAsync(ownerAssetIds, cancellationToken);

        var projectNames = new Dictionary<Guid, string?>();

        foreach (var projectId in page.Items.Select(item => item.ProjectId).Distinct())
        {
            projectNames[projectId] = await _projectLookupService.GetNameAsync(projectId, cancellationToken);
        }

        var enrichedItems = page.Items
            .Select(item => item with
            {
                DeviceCode = assetInfos.TryGetValue(item.OwnerId, out var info) ? info.Code : null,
                ProjectName = projectNames.GetValueOrDefault(item.ProjectId),
            })
            .ToList();

        return page with { Items = enrichedItems };
    }
}
