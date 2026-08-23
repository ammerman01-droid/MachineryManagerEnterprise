using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Projects.Commands.RegisterProject;

/// <summary>
/// Handles <see cref="RegisterProjectCommand"/> by orchestrating domain
/// registration, persisting the aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterProjectCommandHandler
    : IRequestHandler<RegisterProjectCommand, Result<Guid>>
{
    private const string RequiredPermission = "Project.Manage";

    private readonly IProjectRepository _projectRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterProjectCommandHandler"/> class.
    /// </summary>
    /// <param name="projectRepository">The project repository.</param>
    /// <param name="organizationRepository">The organization repository, used to verify the owning Organization exists and to resolve its scope chain.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public RegisterProjectCommandHandler(
        IProjectRepository projectRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _projectRepository = projectRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>
    /// Executes the registration use case.
    /// </summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new project's GUID on success.</returns>
    public async Task<Result<Guid>> Handle(
        RegisterProjectCommand request,
        CancellationToken cancellationToken)
    {
        var organizationId = OrganizationId.From(request.OrganizationId);
        var organization = await _organizationRepository.GetByIdAsync(organizationId, cancellationToken);

        if (organization is null)
        {
            return Result.Failure<Guid>(
                Error.NotFound(
                    "Organization.NotFound",
                    $"Organization with id {request.OrganizationId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(ProjectErrors.NotAuthorized());
        }

        // Full scope chain resolved from the owning Organization, so a
        // Holding Administrator's scoped assignment correctly covers
        // Projects created under any Organization of that Holding.
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
            return Result.Failure<Guid>(ProjectErrors.NotAuthorized());
        }

        var result = Project.Register(organizationId, request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _projectRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}