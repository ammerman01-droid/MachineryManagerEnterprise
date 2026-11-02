using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RegisterMeterDevice;

/// <summary>
/// Handles <see cref="RegisterMeterDeviceCommand"/> by validating the
/// referenced Organization exists, verifying the device name is unique
/// within it, invoking domain registration, persisting the aggregate, and
/// committing the unit of work.
/// </summary>
public sealed class RegisterMeterDeviceCommandHandler
    : IRequestHandler<RegisterMeterDeviceCommand, Result<Guid>>
{
    private const string RequiredPermission = "MeterDevice.Create";

    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterMeterDeviceCommandHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work, used to commit the new aggregate.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to verify the target Organization exists and to resolve its Holding.</param>
    public RegisterMeterDeviceCommandHandler(
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

    /// <summary>Executes the registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterMeterDeviceCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Usage.Domain.MeterDeviceErrors.NotAuthorized());
        }

        var organizationExists = await _organizationLookupService.ExistsAsync(request.OrganizationId, cancellationToken);

        if (!organizationExists)
        {
            return Result.Failure<Guid>(Error.NotFound(
                "MeterDevice.OrganizationNotFound",
                $"Organization {request.OrganizationId} was not found."));
        }

        var name = request.Name?.Trim() ?? string.Empty;

        if (name.Length > 0 && await _meterDeviceRepository.NameExistsAsync(request.OrganizationId, name, null, cancellationToken))
        {
            return Result.Failure<Guid>(global::Usage.Domain.MeterDeviceErrors.NameAlreadyExists(name));
        }

        var result = global::Usage.Domain.MeterDevice.Register(
            request.OrganizationId,
            name,
            request.Unit,
            request.DailyCapOverride);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _meterDeviceRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
