using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.GetMeterDeviceById;

/// <summary>Handles <see cref="GetMeterDeviceByIdQuery"/> by loading the device and projecting it to <see cref="MeterDeviceListItemDto"/>.</summary>
public sealed class GetMeterDeviceByIdQueryHandler
    : IRequestHandler<GetMeterDeviceByIdQuery, Result<MeterDeviceListItemDto>>
{
    private const string RequiredPermission = "MeterDevice.View";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetMeterDeviceByIdQueryHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    public GetMeterDeviceByIdQueryHandler(
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

        return Result.Success(new MeterDeviceListItemDto(
            device.Id.Value,
            device.OrganizationId,
            device.Unit.ToString(),
            device.Status.ToString(),
            device.OwnerType?.ToString(),
            device.OwnerId,
            device.DailyCapOverride));
    }
}
