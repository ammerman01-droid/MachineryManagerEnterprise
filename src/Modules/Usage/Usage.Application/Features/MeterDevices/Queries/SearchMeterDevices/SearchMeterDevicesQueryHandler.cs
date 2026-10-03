using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.SearchMeterDevices;

/// <summary>Handles <see cref="SearchMeterDevicesQuery"/> by authorizing against the given Organization, then delegating to <see cref="IMeterDeviceRepository.SearchAsync"/>.</summary>
public sealed class SearchMeterDevicesQueryHandler
    : IRequestHandler<SearchMeterDevicesQuery, Result<SearchMeterDevicesResponse>>
{
    private const string RequiredPermission = "MeterDevice.View";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="SearchMeterDevicesQueryHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the Organization's Holding.</param>
    public SearchMeterDevicesQueryHandler(
        IMeterDeviceRepository meterDeviceRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
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

        return Result.Success(response);
    }
}
