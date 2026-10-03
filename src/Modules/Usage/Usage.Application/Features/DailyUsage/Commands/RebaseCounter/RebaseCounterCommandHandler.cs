using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Common;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RebaseCounter;

/// <summary>
/// Handles <see cref="RebaseCounterCommand"/>. Loads the Ledger and
/// the replacement device, resolves any missed mandatory shifts up to
/// (but not including) the target date, marks the replacement device
/// Operational, and applies the rebase atomically.
/// </summary>
public sealed class RebaseCounterCommandHandler
    : IRequestHandler<RebaseCounterCommand, Result>
{
    private const string RequiredPermission = "DailyUsage.Rebase";

    private readonly IUsageLedgerRepository _usageLedgerRepository;
    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IWorkCalendarLookupService _workCalendarLookupService;

    /// <summary>Initializes a new instance of the <see cref="RebaseCounterCommandHandler"/> class.</summary>
    /// <param name="usageLedgerRepository">The Usage Ledger repository.</param>
    /// <param name="meterDeviceRepository">The Meter Device repository, used to load the replacement device.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work, used to commit both aggregates together.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain events, and today's date for the future-date check.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the ledger's Holding.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to resolve the owning Asset's current Project.</param>
    /// <param name="workCalendarLookupService">Cross-module, read-only lookup into the WorkCalendar module, used to resolve any missed mandatory shifts.</param>
    public RebaseCounterCommandHandler(
        IUsageLedgerRepository usageLedgerRepository,
        IMeterDeviceRepository meterDeviceRepository,
        IUsageUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService,
        IAssetLookupService assetLookupService,
        IWorkCalendarLookupService workCalendarLookupService)
    {
        _usageLedgerRepository = usageLedgerRepository;
        _meterDeviceRepository = meterDeviceRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
        _assetLookupService = assetLookupService;
        _workCalendarLookupService = workCalendarLookupService;
    }

    /// <summary>Executes the counter rebase use case.</summary>
    public async Task<Result> Handle(RebaseCounterCommand request, CancellationToken cancellationToken)
    {
        var ledger = await _usageLedgerRepository.GetByOwnerAsync(request.OwnerType, request.OwnerId, request.Unit, cancellationToken);

        if (ledger is null)
        {
            return Result.Failure(Error.NotFound(
                "UsageLedger.NotFound", $"No Usage Ledger was found for owner {request.OwnerId}."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(ledger.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, ledger.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var newDeviceId = global::Usage.Domain.MeterDeviceId.From(request.NewMeterDeviceId);
        var newDevice = await _meterDeviceRepository.GetByIdAsync(newDeviceId, cancellationToken);

        if (newDevice is null)
        {
            return Result.Failure(Error.NotFound(
                "MeterDevice.NotFound", $"Meter Device {request.NewMeterDeviceId} was not found."));
        }

        if (newDevice.OwnerType != ledger.OwnerType || newDevice.OwnerId != ledger.OwnerId)
        {
            return Result.Failure(Error.Conflict(
                "MeterDevice.NotInstalledOnOwner",
                "The replacement device must already be installed on this Ledger's owner before rebasing."));
        }

        if (ledger.OwnerType != global::Usage.Domain.UsageOwnerType.Asset)
        {
            return Result.Failure(Error.Failure(
                "MeterDevice.OwnerNotSupported",
                "Counter rebase for Tracked Component owners is not supported yet."));
        }

        var assetContext = await _assetLookupService.GetUsageContextAsync(ledger.OwnerId, cancellationToken);

        if (assetContext is null)
        {
            return Result.Failure(Error.NotFound(
                "Asset.NotFound", $"Asset {ledger.OwnerId} was not found."));
        }

        var lastEntry = ledger.Entries.LastOrDefault();

        var missedShiftsResult = await MandatoryShiftResolver.ResolveAsync(
            _workCalendarLookupService, assetContext.ProjectId, lastEntry, request.TargetDate, toShiftIndexExclusive: null, cancellationToken);

        if (missedShiftsResult.IsFailure)
        {
            return Result.Failure(missedShiftsResult.Error);
        }

        // Step 1 (in memory only): mark the replacement device Operational — this also confirms it is Installed/Operational.
        var markOperationalResult = newDevice.MarkOperational();

        if (markOperationalResult.IsFailure)
        {
            return markOperationalResult;
        }

        var today = DateOnly.FromDateTime(_dateTimeProvider.UtcNow.UtcDateTime);

        // Step 2 (in memory only): apply the rebase.
        var rebaseResult = ledger.RebaseCounter(
            request.NewRawReadingValue,
            newDevice.Id,
            request.TargetDate,
            assetContext.ProjectId,
            missedShiftsResult.Value,
            today,
            _dateTimeProvider);

        if (rebaseResult.IsFailure)
        {
            return rebaseResult;
        }

        _meterDeviceRepository.Update(newDevice);
        _usageLedgerRepository.Update(ledger);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
