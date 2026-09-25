using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Dtos;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Mappings;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Queries.GetLubricantOverflowReportById;

/// <summary>Handles <see cref="GetLubricantOverflowReportByIdQuery"/>.</summary>
public sealed class GetLubricantOverflowReportByIdQueryHandler : IRequestHandler<GetLubricantOverflowReportByIdQuery, Result<LubricantOverflowReportDto>>
{
    private const string RequiredPermission = "LubricantOverflowReport.View";

    private readonly ILubricantOverflowReportRepository _reportRepository;
    private readonly IConsumptionFreezeSettingRepository _freezeSettingRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetLubricantOverflowReportByIdQueryHandler"/> class.</summary>
    public GetLubricantOverflowReportByIdQueryHandler(
        ILubricantOverflowReportRepository reportRepository,
        IConsumptionFreezeSettingRepository freezeSettingRepository,
        IOrganizationLookupService organizationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _reportRepository = reportRepository;
        _freezeSettingRepository = freezeSettingRepository;
        _organizationLookupService = organizationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetLubricantOverflowReportByIdQuery"/>.</summary>
    public async Task<Result<LubricantOverflowReportDto>> Handle(GetLubricantOverflowReportByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<LubricantOverflowReportDto>(LubricantOverflowReportErrors.NotAuthorized());
        }

        var report = await _reportRepository.GetByIdAsync(
            LubricantOverflowReportId.From(request.LubricantOverflowReportId), cancellationToken);

        if (report is null)
        {
            return Result.Failure<LubricantOverflowReportDto>(LubricantOverflowReportErrors.NotFound());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(report.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, report.OrganizationId, report.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<LubricantOverflowReportDto>(LubricantOverflowReportErrors.NotAuthorized());
        }

        var freezeSetting = await _freezeSettingRepository.GetByOrganizationAsync(report.OrganizationId, cancellationToken);

        return Result.Success(report.ToDto(freezeSetting?.ThresholdDate));
    }
}
