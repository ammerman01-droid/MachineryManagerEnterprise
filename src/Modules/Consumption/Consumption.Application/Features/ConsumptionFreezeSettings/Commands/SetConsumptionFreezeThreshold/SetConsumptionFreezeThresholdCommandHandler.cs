using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Commands.SetConsumptionFreezeThreshold;

/// <summary>Handles <see cref="SetConsumptionFreezeThresholdCommand"/>.</summary>
public sealed class SetConsumptionFreezeThresholdCommandHandler : IRequestHandler<SetConsumptionFreezeThresholdCommand, Result>
{
    private const string RequiredPermission = "ConsumptionFreezeSetting.Edit";

    private readonly IConsumptionFreezeSettingRepository _freezeSettingRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IConsumptionUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="SetConsumptionFreezeThresholdCommandHandler"/> class.</summary>
    public SetConsumptionFreezeThresholdCommandHandler(
        IConsumptionFreezeSettingRepository freezeSettingRepository,
        IOrganizationLookupService organizationLookupService,
        IConsumptionUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _freezeSettingRepository = freezeSettingRepository;
        _organizationLookupService = organizationLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="SetConsumptionFreezeThresholdCommand"/>.</summary>
    public async Task<Result> Handle(SetConsumptionFreezeThresholdCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(ConsumptionFreezeSettingErrors.NotAuthorized());
        }

        if (!await _organizationLookupService.ExistsAsync(request.OrganizationId, cancellationToken))
        {
            return Result.Failure(Error.NotFound("Organization.NotFound", $"Organization with id {request.OrganizationId} was not found."));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(ConsumptionFreezeSettingErrors.NotAuthorized());
        }

        var setting = await _freezeSettingRepository.GetByOrganizationAsync(request.OrganizationId, cancellationToken);

        if (setting is null)
        {
            setting = ConsumptionFreezeSetting.Create(request.OrganizationId, request.ThresholdDate, _dateTimeProvider);
            _freezeSettingRepository.Add(setting);
        }
        else
        {
            setting.SetThreshold(request.ThresholdDate, _dateTimeProvider);
            _freezeSettingRepository.Update(setting);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
