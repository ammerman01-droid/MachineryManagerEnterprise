using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Common;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RegisterShiftReading;

/// <summary>
/// Handles <see cref="RegisterShiftReadingCommand"/>. Resolves the
/// device's owner and current Project, resolves the target shift (and
/// any missed mandatory shifts since the Ledger's last entry) from the
/// Work Calendar, marks the device Operational, and registers the
/// reading on the owner's Usage Ledger — all validated in memory
/// before anything is persisted, so a failure at any step leaves
/// neither aggregate changed (BR-010, BR-015).
/// </summary>
public sealed class RegisterShiftReadingCommandHandler
    : IRequestHandler<RegisterShiftReadingCommand, Result>
{
    private const string RequiredPermission = "DailyUsage.Register";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageLedgerRepository _usageLedgerRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IWorkCalendarLookupService _workCalendarLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterShiftReadingCommandHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="usageLedgerRepository">The Usage Ledger repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work, used to commit both aggregates together.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain events.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to resolve the owning Asset's current Project — required to snapshot onto the new entry (BR-054).</param>
    /// <param name="workCalendarLookupService">Cross-module, read-only lookup into the WorkCalendar module, used to resolve the target shift and any missed mandatory shifts.</param>
    public RegisterShiftReadingCommandHandler(
        IMeterDeviceRepository meterDeviceRepository,
        IUsageLedgerRepository usageLedgerRepository,
        IUsageUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService,
        IAssetLookupService assetLookupService,
        IWorkCalendarLookupService workCalendarLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _usageLedgerRepository = usageLedgerRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
        _assetLookupService = assetLookupService;
        _workCalendarLookupService = workCalendarLookupService;
    }

    /// <summary>Executes the shift reading registration use case.</summary>
    public async Task<Result> Handle(RegisterShiftReadingCommand request, CancellationToken cancellationToken)
    {
        var deviceId = global::Usage.Domain.MeterDeviceId.From(request.MeterDeviceId);
        var device = await _meterDeviceRepository.GetByIdAsync(deviceId, cancellationToken);

        if (device is null)
        {
            return Result.Failure(Error.NotFound(
                "MeterDevice.NotFound", $"Meter Device {request.MeterDeviceId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(device.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, device.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        if (device.OwnerType is not global::Usage.Domain.UsageOwnerType.Asset || device.OwnerId is not { } ownerAssetId)
        {
            // BR-054 requires a Project snapshot on every entry; resolving a Tracked Component's
            // current host Asset (and therefore its Project) is not yet supported (see the
            // UsageOwnerType.Component remarks), and a device that is not installed at all has no
            // owner to register usage against.
            return Result.Failure(Error.Failure(
                "MeterDevice.OwnerNotSupported",
                "This device is not installed on an Asset; usage cannot be registered."));
        }

        var assetContext = await _assetLookupService.GetUsageContextAsync(ownerAssetId, cancellationToken);

        if (assetContext is null)
        {
            return Result.Failure(Error.NotFound(
                "Asset.NotFound", $"Asset {ownerAssetId} was not found."));
        }

        var daySchedule = await _workCalendarLookupService.ResolveDayScheduleAsync(assetContext.ProjectId, request.EntryDate, cancellationToken);

        if (daySchedule is null)
        {
            return Result.Failure(MandatoryShiftResolver.CalendarNotResolved(request.EntryDate));
        }

        var targetShift = daySchedule.IsWorkingDay ? daySchedule.Shifts.FirstOrDefault(s => s.Index == request.ShiftIndex) : null;

        if (targetShift is null)
        {
            return Result.Failure(MandatoryShiftResolver.ShiftNotScheduled(request.EntryDate, request.ShiftIndex));
        }

        var ownerType = device.OwnerType.Value;
        var ownerId = device.OwnerId.Value;

        var ledger = await _usageLedgerRepository.GetByOwnerAsync(ownerType, ownerId, device.Unit, cancellationToken);
        var isNewLedger = ledger is null;

        if (ledger is null)
        {
            var createLedgerResult = global::Usage.Domain.UsageLedger.Create(ownerType, ownerId, device.OrganizationId, device.Unit);

            if (createLedgerResult.IsFailure)
            {
                return Result.Failure(createLedgerResult.Error);
            }

            ledger = createLedgerResult.Value;
        }

        var lastEntry = ledger.Entries.LastOrDefault();

        var missedShiftsResult = await MandatoryShiftResolver.ResolveAsync(
            _workCalendarLookupService, assetContext.ProjectId, lastEntry, request.EntryDate, request.ShiftIndex, cancellationToken);

        if (missedShiftsResult.IsFailure)
        {
            return Result.Failure(missedShiftsResult.Error);
        }

        // Step 1 (in memory only): mark the device Operational — this also confirms it is Installed/Operational.
        var markOperationalResult = device.MarkOperational();

        if (markOperationalResult.IsFailure)
        {
            return markOperationalResult;
        }

        // Step 2 (in memory only): register the shift's reading on the ledger.
        var registerResult = ledger.RegisterShiftReading(
            request.EntryDate,
            request.ShiftIndex,
            targetShift.StartTime,
            targetShift.EndTime,
            request.RawReadingValue,
            device.Id,
            request.OperatorId,
            assetContext.ProjectId,
            missedShiftsResult.Value,
            _dateTimeProvider);

        if (registerResult.IsFailure)
        {
            // Neither the device change above nor anything on the ledger has been persisted yet — returning here discards both.
            return registerResult;
        }

        // Both aggregates validated successfully — persist together, in one transaction.
        _meterDeviceRepository.Update(device);

        if (isNewLedger)
        {
            _usageLedgerRepository.Add(ledger);
        }
        else
        {
            _usageLedgerRepository.Update(ledger);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
