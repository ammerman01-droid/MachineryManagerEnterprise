using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Dtos;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Mappings;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Queries.GetLubricantOverflowReportsByAsset;

/// <summary>Handles <see cref="GetLubricantOverflowReportsByAssetQuery"/>.</summary>
public sealed class GetLubricantOverflowReportsByAssetQueryHandler
    : IRequestHandler<GetLubricantOverflowReportsByAssetQuery, Result<IReadOnlyList<LubricantOverflowReportDto>>>
{
    private const string RequiredPermission = "LubricantOverflowReport.View";

    private readonly ILubricantOverflowReportRepository _reportRepository;
    private readonly IConsumptionFreezeSettingRepository _freezeSettingRepository;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetLubricantOverflowReportsByAssetQueryHandler"/> class.</summary>
    public GetLubricantOverflowReportsByAssetQueryHandler(
        ILubricantOverflowReportRepository reportRepository,
        IConsumptionFreezeSettingRepository freezeSettingRepository,
        IAssetLookupService assetLookupService,
        IOrganizationLookupService organizationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _reportRepository = reportRepository;
        _freezeSettingRepository = freezeSettingRepository;
        _assetLookupService = assetLookupService;
        _organizationLookupService = organizationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetLubricantOverflowReportsByAssetQuery"/>.</summary>
    public async Task<Result<IReadOnlyList<LubricantOverflowReportDto>>> Handle(GetLubricantOverflowReportsByAssetQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<LubricantOverflowReportDto>>(global::Consumption.Domain.LubricantOverflowReportErrors.NotAuthorized());
        }

        var organizationId = await _assetLookupService.GetOrganizationIdAsync(request.AssetId, cancellationToken);

        if (organizationId is null)
        {
            return Result.Failure<IReadOnlyList<LubricantOverflowReportDto>>(
                Error.NotFound("Asset.NotFound", $"Asset with id {request.AssetId} was not found."));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(organizationId.Value, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, organizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<LubricantOverflowReportDto>>(global::Consumption.Domain.LubricantOverflowReportErrors.NotAuthorized());
        }

        var freezeSetting = await _freezeSettingRepository.GetByOrganizationAsync(organizationId.Value, cancellationToken);
        var reports = await _reportRepository.GetByAssetAsync(request.AssetId, cancellationToken);

        IReadOnlyList<LubricantOverflowReportDto> dtos = reports
            .Select(r => r.ToDto(freezeSetting?.ThresholdDate))
            .ToList();

        return Result.Success(dtos);
    }
}
