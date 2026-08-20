using MachineryManager.Organization.Application.Features.Projects.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Projects.Queries.GetProjectById;

/// <summary>
/// Query to retrieve a single project by its unique identifier.
/// </summary>
/// <param name="ProjectId">The GUID of the project to retrieve.</param>
public sealed record GetProjectByIdQuery(Guid ProjectId)
    : IRequest<Result<ProjectDto>>;