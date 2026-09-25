using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Queries.GetConsumptionFreezeThreshold;

/// <summary>Handles <see cref="GetConsumptionFreezeThresholdQuery"/>.</summary>
public sealed class GetConsumptionFreezeThresholdQueryHandler : IRequestHandler<GetConsumptionFreezeThresholdQuery, Result<ConsumptionFreezeSettingDto?>>
{
    private const string RequiredPermission = "ConsumptionFreezeSetting.View";

    private readonly IConsumptionFreezeSettingRepository _freezeSettingRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetConsumptionFreezeThresholdQueryHandler"/> class.</summary>
    public GetConsumptionFreezeThresholdQueryHandler(
        IConsumptionFreezeSettingRepository freezeSettingRepository,
        IOrganizationLookupService organizationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _freezeSettingRepository = freezeSettingRepository;
        _organizationLookupService = organizationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetConsumptionFreezeThresholdQuery"/>.</summary>
    public async Task<Result<ConsumptionFreezeSettingDto?>> Handle(GetConsumptionFreezeThresholdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<ConsumptionFreezeSettingDto?>(global::Consumption.Domain.ConsumptionFreezeSettingErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<ConsumptionFreezeSettingDto?>(global::Consumption.Domain.ConsumptionFreezeSettingErrors.NotAuthorized());
        }

        var setting = await _freezeSettingRepository.GetByOrganizationAsync(request.OrganizationId, cancellationToken);

        return Result.Success<ConsumptionFreezeSettingDto?>(
            setting is null ? null : new ConsumptionFreezeSettingDto(setting.OrganizationId, setting.ThresholdDate));
    }
}
