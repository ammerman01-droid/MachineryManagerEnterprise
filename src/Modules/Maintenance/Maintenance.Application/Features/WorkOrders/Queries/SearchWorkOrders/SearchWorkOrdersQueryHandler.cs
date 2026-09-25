using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Dtos;
using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.SearchWorkOrders;

/// <summary>
/// Handles <see cref="SearchWorkOrdersQuery"/> by verifying the caller
/// is authorized for the requested Organization, delegating to the
/// repository search projection, and enriching each item with the
/// Asset's and responsible Personnel's display names via cross-module
/// lookups.
/// </summary>
public sealed class SearchWorkOrdersQueryHandler
    : IRequestHandler<SearchWorkOrdersQuery, Result<SearchWorkOrdersResponse>>
{
    private const string RequiredPermission = "WorkOrder.View";

    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="SearchWorkOrdersQueryHandler"/> class.</summary>
    public SearchWorkOrdersQueryHandler(
        IWorkOrderRepository workOrderRepository,
        IAssetLookupService assetLookupService,
        IPersonnelLookupService personnelLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _workOrderRepository = workOrderRepository;
        _assetLookupService = assetLookupService;
        _personnelLookupService = personnelLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the search query.</summary>
    public async Task<Result<SearchWorkOrdersResponse>> Handle(
        SearchWorkOrdersQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchWorkOrdersResponse>(WorkOrderErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<SearchWorkOrdersResponse>(WorkOrderErrors.NotAuthorized());
        }

        var response = await _workOrderRepository.SearchAsync(
            request.OrganizationId,
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        // Enriched here rather than in the repository (chat, 2026-09-22)
        // — Asset/Personnel names are cross-module display data, out of
        // reach of the Maintenance module's own EF query. A future
        // optimization could batch these lookups; acceptable as
        // per-item calls for a first version.
        var enrichedItems = new List<WorkOrderDto>(response.Items.Count);

        foreach (var item in response.Items)
        {
            var assetCode = await _assetLookupService.GetCodeAsync(item.AssetId, cancellationToken);
            var assetName = await _assetLookupService.GetNameAsync(item.AssetId, cancellationToken);
            var responsibleName = await _personnelLookupService.GetFullNameAsync(item.ResponsiblePersonnelId, cancellationToken);

            enrichedItems.Add(item with
            {
                AssetCode = assetCode,
                AssetName = assetName,
                ResponsiblePersonnelName = responsibleName,
            });
        }

        return Result.Success(response with { Items = enrichedItems });
    }
}
