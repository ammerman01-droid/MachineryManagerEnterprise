using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.DeleteLubricantOverflowReport;

/// <summary>Handles <see cref="DeleteLubricantOverflowReportCommand"/>. Blocked (Conflict) once the report's date has been frozen.</summary>
public sealed class DeleteLubricantOverflowReportCommandHandler : IRequestHandler<DeleteLubricantOverflowReportCommand, Result>
{
    private const string RequiredPermission = "LubricantOverflowReport.Delete";

    private readonly ILubricantOverflowReportRepository _reportRepository;
    private readonly IConsumptionFreezeSettingRepository _freezeSettingRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IConsumptionUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeleteLubricantOverflowReportCommandHandler"/> class.</summary>
    public DeleteLubricantOverflowReportCommandHandler(
        ILubricantOverflowReportRepository reportRepository,
        IConsumptionFreezeSettingRepository freezeSettingRepository,
        IOrganizationLookupService organizationLookupService,
        IConsumptionUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _reportRepository = reportRepository;
        _freezeSettingRepository = freezeSettingRepository;
        _organizationLookupService = organizationLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="DeleteLubricantOverflowReportCommand"/>.</summary>
    public async Task<Result> Handle(DeleteLubricantOverflowReportCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(LubricantOverflowReportErrors.NotAuthorized());
        }

        var report = await _reportRepository.GetByIdAsync(
            LubricantOverflowReportId.From(request.LubricantOverflowReportId), cancellationToken);

        if (report is null)
        {
            return Result.Failure(LubricantOverflowReportErrors.NotFound());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(report.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, report.OrganizationId, report.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(LubricantOverflowReportErrors.NotAuthorized());
        }

        var freezeSetting = await _freezeSettingRepository.GetByOrganizationAsync(report.OrganizationId, cancellationToken);

        var markResult = report.MarkAsDeleted(freezeSetting?.ThresholdDate, _dateTimeProvider);

        if (markResult.IsFailure)
        {
            return markResult;
        }

        _reportRepository.Remove(report);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
