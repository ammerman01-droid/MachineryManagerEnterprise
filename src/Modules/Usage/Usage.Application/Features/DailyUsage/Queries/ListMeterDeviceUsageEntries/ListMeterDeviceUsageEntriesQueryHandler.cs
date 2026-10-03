using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListMeterDeviceUsageEntries;

/// <summary>
/// Handles <see cref="ListMeterDeviceUsageEntriesQuery"/> by
/// authorizing against the Meter Device's Organization, then
/// delegating the actual cross-ledger listing to
/// <see cref="IUsageReadService"/>.
/// </summary>
public sealed class ListMeterDeviceUsageEntriesQueryHandler
    : IRequestHandler<ListMeterDeviceUsageEntriesQuery, Result<PagedResult<UsageEntryListItem>>>
{
    private const string RequiredPermission = "DailyUsage.View";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageReadService _usageReadService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="ListMeterDeviceUsageEntriesQueryHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository, used only to resolve the Organization for authorization.</param>
    /// <param name="usageReadService">The read-only, cross-ledger projection over usage entries.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    public ListMeterDeviceUsageEntriesQueryHandler(
        IMeterDeviceRepository meterDeviceRepository,
        IUsageReadService usageReadService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _usageReadService = usageReadService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the usage-history listing.</summary>
    public async Task<Result<PagedResult<UsageEntryListItem>>> Handle(
        ListMeterDeviceUsageEntriesQuery request, CancellationToken cancellationToken)
    {
        var deviceId = global::Usage.Domain.MeterDeviceId.From(request.MeterDeviceId);
        var device = await _meterDeviceRepository.GetByIdAsync(deviceId, cancellationToken);

        if (device is null)
        {
            return Result.Failure<PagedResult<UsageEntryListItem>>(Error.NotFound(
                "MeterDevice.NotFound", $"Meter Device {request.MeterDeviceId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<PagedResult<UsageEntryListItem>>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(device.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, device.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<PagedResult<UsageEntryListItem>>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var page = await _usageReadService.ListByMeterDeviceAsync(
            request.MeterDeviceId,
            request.FromDate,
            request.ToDate,
            request.ProjectId,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(page);
    }
}
