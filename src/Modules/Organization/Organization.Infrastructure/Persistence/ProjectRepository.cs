using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Projects.Dtos;
using MachineryManager.Organization.Application.Features.Projects.Queries.SearchProjects;
using Microsoft.EntityFrameworkCore;
using Organization.Domain;
using MachineryManager.SharedKernel.Abstractions;

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
    public void Remove(Project aggregate) => _dbContext.Projects.Remove(aggregate);

    /// <inheritdoc />
    public async Task<SearchProjectsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        AuthorizedScopeSet authorizedScope,
        CancellationToken cancellationToken = default)
    {
        // Read-only projection Join with Organization, purely to resolve
        // each Project's parent Holding for scope filtering — Project
        // itself stores no HoldingId (deliberate aggregate-boundary
        // decision, see ProjectConfiguration remarks). This is a query
        // concern, not a permanent cross-aggregate navigation.
        var query =
            from project in _dbContext.Projects.AsNoTracking()
            join organization in _dbContext.Organizations.AsNoTracking()
                on project.OrganizationId equals organization.Id
            select new { Project = project, Organization = organization };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Project.Name.Contains(searchTerm));
        }

        if (!authorizedScope.IsUnrestricted)
        {
            var projectIdSet = authorizedScope.ProjectIds
                .Select(ProjectId.From)
                .ToHashSet();
            var organizationIdSet = authorizedScope.OrganizationIds
                .Select(OrganizationId.From)
                .ToHashSet();
            var holdingIdSet = authorizedScope.HoldingIds
                .Select(HoldingId.From)
                .ToHashSet();

            query = query.Where(x =>
                projectIdSet.Contains(x.Project.Id)
                || organizationIdSet.Contains(x.Project.OrganizationId)
                || (x.Organization.HoldingId != null && holdingIdSet.Contains(x.Organization.HoldingId)));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.Project.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProjectDto(x.Project.Id.Value, x.Project.Name, x.Project.OrganizationId.Value))
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