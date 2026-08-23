using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.AssignOrganizationToHolding;

/// <summary>
/// Handles <see cref="AssignOrganizationToHoldingCommand"/> by loading the
/// organization aggregate, invoking the domain assignment behavior, and
/// committing the unit of work.
/// </summary>
public sealed class AssignOrganizationToHoldingCommandHandler
    : IRequestHandler<AssignOrganizationToHoldingCommand, Result>
{
    private const string RequiredPermission = "Organization.Manage";

    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignOrganizationToHoldingCommandHandler"/> class.
    /// </summary>
    /// <param name="organizationRepository">The organization repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public AssignOrganizationToHoldingCommandHandler(
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

    /// <summary>
    /// Executes the assignment use case.
    /// </summary>
    /// <param name="request">The assignment command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        AssignOrganizationToHoldingCommand request,
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

        // Full scope chain resolved from the already-loaded Organization
        // (its CURRENT Holding, before this reassignment) — per
        // ResourceScope's contract that the calling module resolves the
        // chain from its own local data.
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

        var holdingId = HoldingId.From(request.HoldingId);
        var result = organization.AssignToHolding(holdingId, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _organizationRepository.Update(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}