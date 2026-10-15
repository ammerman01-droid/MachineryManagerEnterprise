using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetLastReading;

/// <summary>Handles <see cref="GetLastReadingQuery"/> by checking permission and reading the last entry of the owner's Usage Ledger.</summary>
public sealed class GetLastReadingQueryHandler : IRequestHandler<GetLastReadingQuery, Result<LastReadingDto>>
{
    private const string RequiredPermission = "DailyUsage.View";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageLedgerRepository _usageLedgerRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetLastReadingQueryHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="usageLedgerRepository">The Usage Ledger repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    public GetLastReadingQueryHandler(
        IMeterDeviceRepository meterDeviceRepository,
        IUsageLedgerRepository usageLedgerRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _usageLedgerRepository = usageLedgerRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the last-reading lookup.</summary>
    public async Task<Result<LastReadingDto>> Handle(GetLastReadingQuery request, CancellationToken cancellationToken)
    {
        var device = await _meterDeviceRepository.GetByIdAsync(
            global::Usage.Domain.MeterDeviceId.From(request.MeterDeviceId), cancellationToken);

        if (device is null)
        {
            return Result.Failure<LastReadingDto>(Error.NotFound(
                "MeterDevice.NotFound", $"Meter Device {request.MeterDeviceId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<LastReadingDto>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(device.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, device.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<LastReadingDto>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        if (device.OwnerType is not { } ownerType || device.OwnerId is not { } ownerId)
        {
            return new LastReadingDto(device.Unit.ToString(), null, null, null, null);
        }

        var ledger = await _usageLedgerRepository.GetByOwnerAsync(ownerType, ownerId, device.Unit, cancellationToken);
        var last = ledger?.Entries.LastOrDefault();

        return new LastReadingDto(
            device.Unit.ToString(),
            last?.RawReadingValue,
            last?.EntryDate,
            last?.ShiftIndex,
            last?.SourceMeterDeviceId.Value);
    }
}
