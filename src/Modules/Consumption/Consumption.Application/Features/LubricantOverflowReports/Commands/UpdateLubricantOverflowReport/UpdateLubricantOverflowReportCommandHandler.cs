using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.UpdateLubricantOverflowReport;

/// <summary>
/// Handles <see cref="UpdateLubricantOverflowReportCommand"/>. Re-validates
/// every Overflow Component, Lubricant Type, and Personnel reference (the
/// caller may have changed the set of lines/entries), enforces the
/// Organization's freeze threshold, and commits the change.
/// </summary>
public sealed class UpdateLubricantOverflowReportCommandHandler : IRequestHandler<UpdateLubricantOverflowReportCommand, Result>
{
    private const string RequiredPermission = "LubricantOverflowReport.Edit";

    private readonly ILubricantOverflowReportRepository _reportRepository;
    private readonly IConsumptionFreezeSettingRepository _freezeSettingRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly IConsumptionUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="UpdateLubricantOverflowReportCommandHandler"/> class.</summary>
    public UpdateLubricantOverflowReportCommandHandler(
        ILubricantOverflowReportRepository reportRepository,
        IConsumptionFreezeSettingRepository freezeSettingRepository,
        IOrganizationLookupService organizationLookupService,
        IConfigurationLookupService configurationLookupService,
        IPersonnelLookupService personnelLookupService,
        IConsumptionUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _reportRepository = reportRepository;
        _freezeSettingRepository = freezeSettingRepository;
        _organizationLookupService = organizationLookupService;
        _configurationLookupService = configurationLookupService;
        _personnelLookupService = personnelLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="UpdateLubricantOverflowReportCommand"/>.</summary>
    public async Task<Result> Handle(UpdateLubricantOverflowReportCommand request, CancellationToken cancellationToken)
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

        if (holdingId is null)
        {
            return Result.Failure(Error.Failure(
                "LubricantOverflowReport.HoldingUnresolved",
                "Could not resolve the owning Holding for this Organization."));
        }

        foreach (var line in request.Lines)
        {
            if (!await _configurationLookupService.OverflowComponentExistsInHoldingAsync(line.OverflowComponentId, holdingId.Value, cancellationToken))
            {
                return Result.Failure(Error.NotFound(
                    "OverflowComponent.NotFound", $"Overflow Component with id {line.OverflowComponentId} was not found."));
            }

            if (!await _configurationLookupService.LubricantTypeExistsInHoldingAsync(line.LubricantTypeId, holdingId.Value, cancellationToken))
            {
                return Result.Failure(Error.NotFound(
                    "LubricantType.NotFound", $"Lubricant Type with id {line.LubricantTypeId} was not found."));
            }
        }

        foreach (var entry in request.PersonnelEntries)
        {
            var personnelOrganizationId = await _personnelLookupService.GetOrganizationIdAsync(entry.PersonnelId, cancellationToken);

            if (personnelOrganizationId is null)
            {
                return Result.Failure(Error.NotFound("Personnel.NotFound", $"Personnel with id {entry.PersonnelId} was not found."));
            }

            if (personnelOrganizationId != report.OrganizationId)
            {
                return Result.Failure(Error.Validation(
                    "LubricantOverflowReport.PersonnelOrganizationMismatch",
                    "The selected Personnel does not belong to the same Organization as the Asset."));
            }
        }

        var freezeSetting = await _freezeSettingRepository.GetByOrganizationAsync(report.OrganizationId, cancellationToken);

        var updateResult = report.Update(
            request.ReportDate,
            request.HourMeterReading,
            request.Lines.Select(l => (l.OverflowComponentId, l.LubricantTypeId, l.AmountInLiters, l.Reason)).ToList(),
            request.PersonnelEntries.Select(e => (e.PersonnelId, e.StartTime, e.Duration)).ToList(),
            freezeSetting?.ThresholdDate,
            _dateTimeProvider);

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
