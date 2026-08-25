using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.ReactivateOrganization;

/// <summary>
/// Handles <see cref="ReactivateOrganizationCommand"/> by loading the
/// organization aggregate, invoking domain reactivation, and
/// committing the unit of work.
/// </summary>
public sealed class ReactivateOrganizationCommandHandler
    : IRequestHandler<ReactivateOrganizationCommand, Result>
{
    private const string RequiredPermission = "Organization.Edit";

    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="ReactivateOrganizationCommandHandler"/> class.</summary>
    /// <param name="organizationRepository">The organization repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public ReactivateOrganizationCommandHandler(
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork,
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

    /// <summary>Executes the reactivation use case.</summary>
    /// <param name="request">The reactivation command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        ReactivateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organizationId = OrganizationId.From(request.OrganizationId);
        var organization = await _organizationRepository.GetByIdAsync(organizationId, cancellationToken);

        if (organization is null)
        {
            return Result.Failure(
                Error.NotFound(
                    "Organization.NotFound",
                    $"Organization with id {request.OrganizationId} was not found."));
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

        var result = organization.Reactivate(_dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _organizationRepository.Update(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}