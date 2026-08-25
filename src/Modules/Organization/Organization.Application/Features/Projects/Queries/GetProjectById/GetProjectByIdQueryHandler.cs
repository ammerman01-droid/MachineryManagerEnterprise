using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Projects.Dtos;
using MachineryManager.SharedKernel;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Projects.Queries.GetProjectById;

/// <summary>
/// Handles <see cref="GetProjectByIdQuery"/> by loading the aggregate
/// and projecting it into a read-only DTO.
/// </summary>
public sealed class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="projectRepository">The project repository.</param>
    public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Executes the query and returns the project DTO if found.
    /// </summary>
    /// <param name="request">The query containing the project identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="ProjectDto"/> or a not-found error.</returns>
    public async Task<Result<ProjectDto>> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var projectId = ProjectId.From(request.ProjectId);
        var project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectDto>(
                Error.NotFound(
                    "Project.NotFound",
                    $"Project with id {request.ProjectId} was not found."));
        }

        var dto = new ProjectDto(
            project.Id.Value,
            project.Name,
            project.OrganizationId.Value);

        return Result.Success(dto);
    }
}