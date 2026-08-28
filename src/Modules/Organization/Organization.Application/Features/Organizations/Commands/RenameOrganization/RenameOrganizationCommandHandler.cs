using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.RenameOrganization;

/// <summary>
/// Handles <see cref="RenameOrganizationCommand"/> by loading the
/// organization aggregate, invoking the domain rename behavior, and
/// committing the unit of work.
/// </summary>
public sealed class RenameOrganizationCommandHandler
    : IRequestHandler<RenameOrganizationCommand, Result>
{
    private const string RequiredPermission = "Organization.Edit";

    private readonly IOrganizationRepository _organizationRepository;
    private readonly IOrganizationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RenameOrganizationCommandHandler"/> class.</summary>
    public RenameOrganizationCommandHandler(
        IOrganizationRepository organizationRepository,
        IOrganizationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the rename use case.</summary>
    public async Task<Result> Handle(RenameOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organizationId = OrganizationId.From(request.OrganizationId);
        var organization = await _organizationRepository.GetByIdAsync(organizationId, cancellationToken);

        if (organization is null)
        {
            return Result.Failure(
                Error.NotFound("Organization.NotFound", $"Organization with id {request.OrganizationId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(OrganizationErrors.NotAuthorized());
        }

        var resourceScope = new ResourceScope(
            organization.HoldingId?.Value,
            organization.Id.Value,
            null);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            resourceScope,
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(OrganizationErrors.NotAuthorized());
        }

        var result = organization.Rename(request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _organizationRepository.Update(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}