using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.GetMeterDeviceById;

/// <summary>
/// Handles <see cref="GetMeterDeviceByIdQuery"/> by loading the device and projecting it to
/// <see cref="MeterDeviceListItemDto"/>, enriched with the owning Asset's code and name
/// (chat, 2026-10-28) so the details page never has to show a raw identifier.
/// </summary>
public sealed class GetMeterDeviceByIdQueryHandler
    : IRequestHandler<GetMeterDeviceByIdQuery, Result<MeterDeviceListItemDto>>
{
    private const string RequiredPermission = "MeterDevice.View";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IAssetLookupService _assetLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetMeterDeviceByIdQueryHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to resolve the owning Asset's code and name.</param>
    public GetMeterDeviceByIdQueryHandler(
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

    /// <summary>Executes the device lookup.</summary>
    public async Task<Result<MeterDeviceListItemDto>> Handle(GetMeterDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var id = global::Usage.Domain.MeterDeviceId.From(request.MeterDeviceId);
        var device = await _meterDeviceRepository.GetByIdAsync(id, cancellationToken);

        if (device is null)
        {
            return Result.Failure<MeterDeviceListItemDto>(Error.NotFound(
                "MeterDevice.NotFound", $"Meter Device {request.MeterDeviceId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<MeterDeviceListItemDto>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(device.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, device.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<MeterDeviceListItemDto>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        string? ownerCode = null;
        string? ownerName = null;

        if (device.OwnerType == global::Usage.Domain.UsageOwnerType.Asset && device.OwnerId is { } ownerId)
        {
            var infos = await _assetLookupService.GetDisplayInfosAsync([ownerId], cancellationToken);

            if (infos.TryGetValue(ownerId, out var info))
            {
                ownerCode = info.Code;
                ownerName = info.Name;
            }
        }

        return Result.Success(new MeterDeviceListItemDto(
            device.Id.Value,
            device.OrganizationId,
            device.Name,
            device.Unit.ToString(),
            device.Status.ToString(),
            device.OwnerType?.ToString(),
            device.OwnerId,
            device.DailyCapOverride,
            ownerCode,
            ownerName));
    }
}
