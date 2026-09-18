using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.RegisterPersonnel;

/// <summary>
/// Handles <see cref="RegisterPersonnelCommand"/>. Validates Organization
/// and Project existence, confirms the Project belongs to the given
/// Organization (BR-017), resolves the Organization's Holding to check
/// the Job Title and Driving License Type catalogs, enforces
/// per-Organization PersonnelCode uniqueness, and checks authorization
/// before delegating to the aggregate.
/// </summary>
public sealed class RegisterPersonnelCommandHandler : IRequestHandler<RegisterPersonnelCommand, Result<Guid>>
{
    private const string RequiredPermission = "Personnel.Create";

    private readonly IPersonnelRepository _personnelRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IProjectLookupService _projectLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IPersonnelUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterPersonnelCommandHandler"/> class.</summary>
    public RegisterPersonnelCommandHandler(
        IPersonnelRepository personnelRepository,
        IOrganizationLookupService organizationLookupService,
        IProjectLookupService projectLookupService,
        IConfigurationLookupService configurationLookupService,
        IPersonnelUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _personnelRepository = personnelRepository;
        _organizationLookupService = organizationLookupService;
        _projectLookupService = projectLookupService;
        _configurationLookupService = configurationLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="RegisterPersonnelCommand"/>.</summary>
    public async Task<Result<Guid>> Handle(RegisterPersonnelCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        if (!await _organizationLookupService.ExistsAsync(request.OrganizationId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("Organization.NotFound", $"Organization with id {request.OrganizationId} was not found."));
        }

        if (!await _projectLookupService.ExistsAsync(request.ProjectId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("Project.NotFound", $"Project with id {request.ProjectId} was not found."));
        }

        var projectOrganizationId = await _projectLookupService.GetOrganizationIdAsync(request.ProjectId, cancellationToken);

        if (projectOrganizationId != request.OrganizationId)
        {
            return Result.Failure<Guid>(Error.Validation("Project.NotInOrganization", "The given Project does not belong to the given Organization."));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        // Flagged, pre-existing architecture gap: Job Title and Driving
        // License Type are Holding-scoped catalogs, but BR-017 allows a
        // standalone Organization with no Holding. Same limitation as
        // every other Holding-scoped catalog.
        if (holdingId is null)
        {
            return Result.Failure<Guid>(Error.Validation("Organization.NoHolding", "The Organization must belong to a Holding to use Job Title / Driving License Type catalog entries."));
        }

        if (!await _configurationLookupService.JobTitleExistsInHoldingAsync(request.JobTitleId, holdingId.Value, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("JobTitle.NotFound", $"Job Title with id {request.JobTitleId} was not found in this Holding."));
        }

        foreach (var license in request.DrivingLicenses)
        {
            if (!await _configurationLookupService.DrivingLicenseTypeExistsInHoldingAsync(license.DrivingLicenseTypeId, holdingId.Value, cancellationToken))
            {
                return Result.Failure<Guid>(Error.NotFound("DrivingLicenseType.NotFound", $"Driving License Type with id {license.DrivingLicenseTypeId} was not found in this Holding."));
            }
        }

        var scope = new ResourceScope(holdingId, request.OrganizationId, request.ProjectId);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        if (await _personnelRepository.ExistsByCodeInOrganizationAsync(request.OrganizationId, request.PersonnelCode, cancellationToken))
        {
            return Result.Failure<Guid>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.PersonnelCodeAlreadyExists());
        }

        var result = global::MachineryManagerEnterprise.Personnel.Domain.Personnel.Register(
            request.OrganizationId,
            request.ProjectId,
            request.FirstName,
            request.LastName,
            request.PersonnelCode,
            request.JobTitleId,
            request.DrivingLicenses.Select(l => (l.DrivingLicenseTypeId, l.ExpiryDate)).ToList(),
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _personnelRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}