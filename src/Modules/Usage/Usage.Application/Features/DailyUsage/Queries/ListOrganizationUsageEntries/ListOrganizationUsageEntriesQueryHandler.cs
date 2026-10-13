using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListOrganizationUsageEntries;

/// <summary>Handles <see cref="ListOrganizationUsageEntriesQuery"/> by checking permission and delegating to <see cref="IUsageReadService.ListByOrganizationAsync"/>.</summary>
public sealed class ListOrganizationUsageEntriesQueryHandler
    : IRequestHandler<ListOrganizationUsageEntriesQuery, Result<PagedResult<UsageEntryListItem>>>
{
    private const string RequiredPermission = "DailyUsage.View";

    private readonly IUsageReadService _usageReadService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="ListOrganizationUsageEntriesQueryHandler"/> class.</summary>
    /// <param name="usageReadService">The Usage module's read-only projection service.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the Organization's Holding.</param>
    public ListOrganizationUsageEntriesQueryHandler(
        IUsageReadService usageReadService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _usageReadService = usageReadService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
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

        var result = await _usageReadService.ListByOrganizationAsync(
            request.OrganizationId,
            request.FromDate,
            request.ToDate,
            request.ProjectId,
            request.MeterDeviceId,
            request.Page,
            request.PageSize,
            cancellationToken);

        return result;
    }
}
