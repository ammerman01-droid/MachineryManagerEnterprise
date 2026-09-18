using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.UpdatePersonnel;

/// <summary>
/// Handles <see cref="UpdatePersonnelCommand"/>. OrganizationId and
/// CurrentProjectId are not editable here (see <see cref="global::MachineryManagerEnterprise.Personnel.Domain.Personnel.UpdateDetails"/>).
/// </summary>
public sealed class UpdatePersonnelCommandHandler : IRequestHandler<UpdatePersonnelCommand, Result>
{
    private const string RequiredPermission = "Personnel.Edit";

    private readonly IPersonnelRepository _personnelRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IPersonnelUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="UpdatePersonnelCommandHandler"/> class.</summary>
    public UpdatePersonnelCommandHandler(
        IPersonnelRepository personnelRepository,
        IOrganizationLookupService organizationLookupService,
        IConfigurationLookupService configurationLookupService,
        IPersonnelUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _personnelRepository = personnelRepository;
        _organizationLookupService = organizationLookupService;
        _configurationLookupService = configurationLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="UpdatePersonnelCommand"/>.</summary>
    public async Task<Result> Handle(UpdatePersonnelCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        var entity = await _personnelRepository.GetByIdAsync(
            global::MachineryManagerEnterprise.Personnel.Domain.PersonnelId.From(request.PersonnelId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotFound());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(entity.OrganizationId, cancellationToken);

        if (holdingId is null)
        {
            return Result.Failure(Error.Validation("Organization.NoHolding", "The Organization must belong to a Holding to use Job Title / Driving License Type catalog entries."));
        }

        if (!await _configurationLookupService.JobTitleExistsInHoldingAsync(request.JobTitleId, holdingId.Value, cancellationToken))
        {
            return Result.Failure(Error.NotFound("JobTitle.NotFound", $"Job Title with id {request.JobTitleId} was not found in this Holding."));
        }

        foreach (var license in request.DrivingLicenses)
        {
            if (!await _configurationLookupService.DrivingLicenseTypeExistsInHoldingAsync(license.DrivingLicenseTypeId, holdingId.Value, cancellationToken))
            {
                return Result.Failure(Error.NotFound("DrivingLicenseType.NotFound", $"Driving License Type with id {license.DrivingLicenseTypeId} was not found in this Holding."));
            }
        }

        var scope = new ResourceScope(holdingId, entity.OrganizationId, entity.CurrentProjectId);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        if (await _personnelRepository.ExistsByCodeInOrganizationAsync(entity.OrganizationId, request.PersonnelCode, request.PersonnelId, cancellationToken))
        {
            return Result.Failure(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.PersonnelCodeAlreadyExists());
        }

        var updateResult = entity.UpdateDetails(request.FirstName, request.LastName, request.PersonnelCode, request.JobTitleId, _dateTimeProvider);

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        var replaceLicensesResult = entity.ReplaceDrivingLicenses(request.DrivingLicenses);

        if (replaceLicensesResult.IsFailure)
        {
            return replaceLicensesResult;
        }

        _personnelRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}