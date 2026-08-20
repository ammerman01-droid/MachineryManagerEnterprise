using MachineryManager.SharedKernel.Abstractions;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Abstractions;

/// <summary>Repository contract for the <see cref="Project"/> aggregate.</summary>
public interface IProjectRepository : IRepository<Project, ProjectId>
{
    /// <summary>Performs a paginated search over projects.</summary>
    Task<Features.Projects.Queries.SearchProjects.SearchProjectsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}