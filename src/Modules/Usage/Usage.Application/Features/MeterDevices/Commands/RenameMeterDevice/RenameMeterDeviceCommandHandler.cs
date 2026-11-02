using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RenameMeterDevice;

/// <summary>
/// Handles <see cref="RenameMeterDeviceCommand"/> by authorizing against the
/// device's Organization, verifying the new name is not taken by another
/// device there, renaming the aggregate, and committing the unit of work.
/// </summary>
public sealed class RenameMeterDeviceCommandHandler : IRequestHandler<RenameMeterDeviceCommand, Result>
{
    private const string RequiredPermission = "MeterDevice.Edit";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RenameMeterDeviceCommandHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the device's Holding.</param>
    public RenameMeterDeviceCommandHandler(
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

    /// <summary>Executes the rename use case.</summary>
    public async Task<Result> Handle(RenameMeterDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _meterDeviceRepository.GetByIdAsync(
            global::Usage.Domain.MeterDeviceId.From(request.MeterDeviceId), cancellationToken);

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

        var name = request.Name?.Trim() ?? string.Empty;

        if (name.Length > 0
            && await _meterDeviceRepository.NameExistsAsync(device.OrganizationId, name, device.Id.Value, cancellationToken))
        {
            return Result.Failure(global::Usage.Domain.MeterDeviceErrors.NameAlreadyExists(name));
        }

        var result = device.Rename(name);

        if (result.IsFailure)
        {
            return result;
        }

        _meterDeviceRepository.Update(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
