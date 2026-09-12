using MachineryManagerEnterprise.Organization.Application.Abstractions;
using MachineryManagerEnterprise.Organization.Application.Features.Projects.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MapsterMapper;
using MediatR;
using Organization.Domain;

namespace MachineryManagerEnterprise.Organization.Application.Features.Projects.Queries.GetProjectById;

/// <summary>
/// Handles <see cref="GetProjectByIdQuery"/> by loading the aggregate
/// and projecting it into a read-only DTO.
/// </summary>
public sealed class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="projectRepository">The project repository.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetProjectByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
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

        return Result.Success(_mapper.Map<ProjectDto>(project));
    }
}