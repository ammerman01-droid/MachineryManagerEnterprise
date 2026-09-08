using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Projects.Commands.RenameProject;

/// <summary>
/// Handles <see cref="RenameProjectCommand"/> by loading the project
/// aggregate, invoking the domain rename behavior, and committing the
/// unit of work.
/// </summary>
public sealed class RenameProjectCommandHandler
    : IRequestHandler<RenameProjectCommand, Result>
{
    private const string RequiredPermission = "Project.Edit";

    private readonly IProjectRepository _projectRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IOrganizationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RenameProjectCommandHandler"/> class.</summary>
    public RenameProjectCommandHandler(
        IProjectRepository projectRepository,
        IOrganizationRepository organizationRepository,
        IOrganizationUnitOfWork unitOfWork,
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

    /// <summary>Executes the rename use case.</summary>
    public async Task<Result> Handle(RenameProjectCommand request, CancellationToken cancellationToken)
    {
        var projectId = ProjectId.From(request.ProjectId);
        var project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure(
                Error.NotFound("Project.NotFound", $"Project with id {request.ProjectId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(ProjectErrors.NotAuthorized());
        }

        // Resolve the full scope chain via the owning Organization, so a
        // Holding/Organization Administrator's assignment correctly
        // covers renaming Projects beneath them.
        var organization = await _organizationRepository.GetByIdAsync(project.OrganizationId, cancellationToken);

        var resourceScope = new ResourceScope(
            organization?.HoldingId?.Value,
            project.OrganizationId.Value,
            project.Id.Value);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            resourceScope,
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(ProjectErrors.NotAuthorized());
        }

        var result = project.Rename(request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}