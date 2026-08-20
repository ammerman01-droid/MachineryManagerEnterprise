using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Projects.Dtos;
using MachineryManager.Organization.Application.Features.Projects.Queries.SearchProjects;
using Microsoft.EntityFrameworkCore;
using Organization.Domain;

namespace MachineryManager.Organization.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IProjectRepository"/>.
/// </summary>
public sealed class ProjectRepository : IProjectRepository
{
    private readonly OrganizationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The Organization module's persistence context.</param>
    public ProjectRepository(OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<Project?> GetByIdAsync(ProjectId id, CancellationToken cancellationToken = default) =>
        _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(Project aggregate) => _dbContext.Projects.Add(aggregate);

    /// <inheritdoc />
    public void Update(Project aggregate) => _dbContext.Projects.Update(aggregate);

    /// <inheritdoc />
    public async Task<SearchProjectsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Projects.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectDto(p.Id.Value, p.Name, p.OrganizationId.Value))
            .ToListAsync(cancellationToken);

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchProjectsResponse(
            items,
            page,
            pageSize,
            totalItems,
            totalPages,
            page < totalPages,
            page > 1);
    }
}