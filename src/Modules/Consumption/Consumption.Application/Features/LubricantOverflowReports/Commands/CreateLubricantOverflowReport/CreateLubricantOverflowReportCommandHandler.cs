using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.CreateLubricantOverflowReport;

/// <summary>
/// Handles <see cref="CreateLubricantOverflowReportCommand"/> by
/// validating the referenced Asset, Project, Lubricant Types, Overflow
/// Components, and Personnel all exist and are consistent with each
/// other's Organization, invoking domain creation, persisting the
/// aggregate, and committing the unit of work.
/// </summary>
public sealed class CreateLubricantOverflowReportCommandHandler
    : IRequestHandler<CreateLubricantOverflowReportCommand, Result<Guid>>
{
    private const string RequiredPermission = "LubricantOverflowReport.Create";

    private readonly ILubricantOverflowReportRepository _reportRepository;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IProjectLookupService _projectLookupService;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly IConsumptionUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="CreateLubricantOverflowReportCommandHandler"/> class.</summary>
    public CreateLubricantOverflowReportCommandHandler(
        ILubricantOverflowReportRepository reportRepository,
        IAssetLookupService assetLookupService,
        IProjectLookupService projectLookupService,
        IOrganizationLookupService organizationLookupService,
        IConfigurationLookupService configurationLookupService,
        IPersonnelLookupService personnelLookupService,
        IConsumptionUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _reportRepository = reportRepository;
        _assetLookupService = assetLookupService;
        _projectLookupService = projectLookupService;
        _organizationLookupService = organizationLookupService;
        _configurationLookupService = configurationLookupService;
        _personnelLookupService = personnelLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="CreateLubricantOverflowReportCommand"/>.</summary>
    public async Task<Result<Guid>> Handle(CreateLubricantOverflowReportCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(LubricantOverflowReportErrors.NotAuthorized());
        }

        var organizationId = await _assetLookupService.GetOrganizationIdAsync(request.AssetId, cancellationToken);

        if (organizationId is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Asset.NotFound", $"Asset with id {request.AssetId} was not found."));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(organizationId.Value, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, organizationId, request.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(LubricantOverflowReportErrors.NotAuthorized());
        }

        if (!await _projectLookupService.ExistsAsync(request.ProjectId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("Project.NotFound", $"Project with id {request.ProjectId} was not found."));
        }

        var projectOrganizationId = await _projectLookupService.GetOrganizationIdAsync(request.ProjectId, cancellationToken);

        if (projectOrganizationId != organizationId.Value)
        {
            return Result.Failure<Guid>(Error.Validation(
                "LubricantOverflowReport.ProjectOrganizationMismatch",
                "The selected Project does not belong to the same Organization as the Asset."));
        }

        if (holdingId is null)
        {
            return Result.Failure<Guid>(Error.Failure(
                "LubricantOverflowReport.HoldingUnresolved",
                "Could not resolve the owning Holding for this Organization."));
        }

        foreach (var line in request.Lines)
        {
            if (!await _configurationLookupService.OverflowComponentExistsInHoldingAsync(line.OverflowComponentId, holdingId.Value, cancellationToken))
            {
                return Result.Failure<Guid>(Error.NotFound(
                    "OverflowComponent.NotFound", $"Overflow Component with id {line.OverflowComponentId} was not found."));
            }

            if (!await _configurationLookupService.LubricantTypeExistsInHoldingAsync(line.LubricantTypeId, holdingId.Value, cancellationToken))
            {
                return Result.Failure<Guid>(Error.NotFound(
                    "LubricantType.NotFound", $"Lubricant Type with id {line.LubricantTypeId} was not found."));
            }
        }

        foreach (var entry in request.PersonnelEntries)
        {
            var personnelOrganizationId = await _personnelLookupService.GetOrganizationIdAsync(entry.PersonnelId, cancellationToken);

            if (personnelOrganizationId is null)
            {
                return Result.Failure<Guid>(Error.NotFound("Personnel.NotFound", $"Personnel with id {entry.PersonnelId} was not found."));
            }

            if (personnelOrganizationId != organizationId.Value)
            {
                return Result.Failure<Guid>(Error.Validation(
                    "LubricantOverflowReport.PersonnelOrganizationMismatch",
                    "The selected Personnel does not belong to the same Organization as the Asset."));
            }
        }

        var result = LubricantOverflowReport.Create(
            organizationId.Value,
            request.ProjectId,
            request.AssetId,
            request.ReportDate,
            request.HourMeterReading,
            request.Lines.Select(l => (l.OverflowComponentId, l.LubricantTypeId, l.AmountInLiters, l.Reason)).ToList(),
            request.PersonnelEntries.Select(e => (e.PersonnelId, e.StartTime, e.Duration)).ToList(),
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _reportRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
