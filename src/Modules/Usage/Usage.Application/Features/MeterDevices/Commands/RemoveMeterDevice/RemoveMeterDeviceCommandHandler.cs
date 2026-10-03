using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RemoveMeterDevice;

/// <summary>
/// Handles <see cref="RemoveMeterDeviceCommand"/> by loading the
/// device, invoking the domain transition to
/// <see cref="global::Usage.Domain.MeterDeviceStatus.Removed"/>, and
/// committing the unit of work.
/// </summary>
public sealed class RemoveMeterDeviceCommandHandler
    : IRequestHandler<RemoveMeterDeviceCommand, Result>
{
    private const string RequiredPermission = "MeterDevice.Remove";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RemoveMeterDeviceCommandHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    public RemoveMeterDeviceCommandHandler(
        IMeterDeviceRepository meterDeviceRepository,
        IUsageUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the removal use case.</summary>
    public async Task<Result> Handle(RemoveMeterDeviceCommand request, CancellationToken cancellationToken)
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

        var result = device.Remove(_dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _meterDeviceRepository.Update(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
