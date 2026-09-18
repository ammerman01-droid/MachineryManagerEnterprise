using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Organization.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IProjectLookupService"/>,
/// mirroring <see cref="OrganizationLookupService"/> and
/// <see cref="HoldingLookupService"/> — a small read-only cross-module
/// contract so other modules (e.g. WorkCalendar) can resolve a
/// Project's Organization/Holding ownership chain without depending on
/// Organization.Domain/Infrastructure directly.
/// </summary>
public sealed class ProjectLookupService : IProjectLookupService
{
    private readonly Persistence.OrganizationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="ProjectLookupService"/> class.</summary>
    /// <param name="dbContext">The Organization module's persistence context.</param>
    public ProjectLookupService(Persistence.OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid projectId, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<global::Organization.Domain.Project>()
            .AnyAsync(p => p.Id == global::Organization.Domain.ProjectId.From(projectId), cancellationToken);

    /// <inheritdoc />
    public async Task<Guid?> GetOrganizationIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Set<global::Organization.Domain.Project>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == global::Organization.Domain.ProjectId.From(projectId), cancellationToken);

        return project?.OrganizationId.Value;
    }

    /// <inheritdoc />
    public async Task<Guid?> GetHoldingIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Set<global::Organization.Domain.Project>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == global::Organization.Domain.ProjectId.From(projectId), cancellationToken);

        if (project is null)
        {
            return null;
        }

        var organization = await _dbContext.Set<global::Organization.Domain.Organization>()
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == project.OrganizationId, cancellationToken);

        return organization?.HoldingId?.Value;
    }

    /// <inheritdoc />
    public async Task<string?> GetNameAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Set<global::Organization.Domain.Project>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == global::Organization.Domain.ProjectId.From(projectId), cancellationToken);

        return project?.Name;
    }
}