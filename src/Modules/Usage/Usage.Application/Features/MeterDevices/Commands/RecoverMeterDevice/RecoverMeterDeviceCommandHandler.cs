using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RecoverMeterDevice;

/// <summary>
/// Handles <see cref="RecoverMeterDeviceCommand"/> by loading the
/// device, invoking the domain transition back to
/// <see cref="global::Usage.Domain.MeterDeviceStatus.Installed"/>, and
/// committing the unit of work. This transition raises no Domain Event
/// (see <see cref="global::Usage.Domain.MeterDevice.Recover"/>).
/// </summary>
public sealed class RecoverMeterDeviceCommandHandler
    : IRequestHandler<RecoverMeterDeviceCommand, Result>
{
    private const string RequiredPermission = "MeterDevice.Recover";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RecoverMeterDeviceCommandHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    public RecoverMeterDeviceCommandHandler(
        IMeterDeviceRepository meterDeviceRepository,
        IUsageUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the recovery use case.</summary>
    public async Task<Result> Handle(RecoverMeterDeviceCommand request, CancellationToken cancellationToken)
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

        var result = device.Recover();

        if (result.IsFailure)
        {
            return result;
        }

        _meterDeviceRepository.Update(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
