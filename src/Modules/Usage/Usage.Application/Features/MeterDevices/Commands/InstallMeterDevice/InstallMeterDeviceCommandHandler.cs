using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.InstallMeterDevice;

/// <summary>
/// Handles <see cref="InstallMeterDeviceCommand"/> by loading the
/// device, confirming the owner exists and belongs to the device's own
/// Organization, invoking domain installation, and committing the
/// unit of work.
/// </summary>
public sealed class InstallMeterDeviceCommandHandler
    : IRequestHandler<InstallMeterDeviceCommand, Result>
{
    private const string RequiredPermission = "MeterDevice.Install";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IAssetLookupService _assetLookupService;

    /// <summary>Initializes a new instance of the <see cref="InstallMeterDeviceCommandHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to confirm an Asset owner exists and belongs to the device's own Organization.</param>
    public InstallMeterDeviceCommandHandler(
        IMeterDeviceRepository meterDeviceRepository,
        IUsageUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService,
        IAssetLookupService assetLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
        _assetLookupService = assetLookupService;
    }

    /// <summary>Executes the installation use case.</summary>
    public async Task<Result> Handle(InstallMeterDeviceCommand request, CancellationToken cancellationToken)
    {
        var id = global::Usage.Domain.MeterDeviceId.From(request.MeterDeviceId);
        var device = await _meterDeviceRepository.GetByIdAsync(id, cancellationToken);

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

        if (request.OwnerType == global::Usage.Domain.UsageOwnerType.Component)
        {
            // The Component module does not exist yet (chat, 2026-09-12) — see BR-004 for the intended design.
            return Result.Failure(global::Usage.Domain.MeterDeviceErrors.ComponentOwnerNotYetSupported());
        }

        var ownerContext = await _assetLookupService.GetUsageContextAsync(request.OwnerId, cancellationToken);

        if (ownerContext is null)
        {
            return Result.Failure(global::Usage.Domain.MeterDeviceErrors.AssetNotFound(request.OwnerId));
        }

        if (ownerContext.OrganizationId != device.OrganizationId)
        {
            return Result.Failure(global::Usage.Domain.MeterDeviceErrors.OwnerOrganizationMismatch());
        }

        if (ownerContext.MeterReadingUnit is not { } ownerMeterReadingUnit)
        {
            return Result.Failure(global::Usage.Domain.MeterDeviceErrors.AssetHasNoMeterReadingUnit(request.OwnerId));
        }

        var result = device.Install(request.OwnerType, request.OwnerId, ownerMeterReadingUnit, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _meterDeviceRepository.Update(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
