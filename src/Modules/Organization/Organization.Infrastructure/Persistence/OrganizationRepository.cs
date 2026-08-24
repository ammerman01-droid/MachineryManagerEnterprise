using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Organizations.Dtos;
using MachineryManager.Organization.Application.Features.Organizations.Queries.SearchOrganizations;
using Microsoft.EntityFrameworkCore;
using Organization.Domain;
using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Organization.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IOrganizationRepository"/>.
/// Per ADR-0019 (Hybrid Persistence Strategy), EF Core is the default
/// for all reads and writes here; Dapper remains opt-in per query and
/// is not used in this repository.
/// </summary>
public sealed class OrganizationRepository : IOrganizationRepository
{
    private readonly OrganizationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="OrganizationRepository"/> class.</summary>
    /// <param name="dbContext">The Organization module's persistence context.</param>
    public OrganizationRepository(OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<global::Organization.Domain.Organization?> GetByIdAsync(
        OrganizationId id,
        CancellationToken cancellationToken = default) =>
        _dbContext.Organizations.FirstOrDefaultAsync(organization => organization.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(global::Organization.Domain.Organization aggregate) =>
        _dbContext.Organizations.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::Organization.Domain.Organization aggregate) =>
        _dbContext.Organizations.Update(aggregate);

    /// <inheritdoc />
    public async Task<SearchOrganizationsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        AuthorizedScopeSet authorizedScope,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Organizations.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(organization => organization.Name.Contains(searchTerm));
        }

        if (!authorizedScope.IsUnrestricted)
        {
            // A user sees an Organization if they were granted access to
            // it directly, or to the Holding it belongs to (top-down
            // visibility only — chat, 2026-08-23). Comparing via the
            // strongly-typed Ids, same as elsewhere in this repository,
            // so EF Core applies the same value converters as the column.
            var organizationIdSet = authorizedScope.OrganizationIds
                .Select(OrganizationId.From)
                .ToHashSet();
            var holdingIdSet = authorizedScope.HoldingIds
                .Select(HoldingId.From)
                .ToHashSet();

            query = query.Where(organization =>
                organizationIdSet.Contains(organization.Id)
                || (organization.HoldingId != null && holdingIdSet.Contains(organization.HoldingId)));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(organization => organization.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(organization => new OrganizationDto(
                organization.Id.Value,
                organization.Name,
                organization.IsSuspended))
            .ToListAsync(cancellationToken);

        var totalPages = totalItems == 0
            ? 0
            : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchOrganizationsResponse(
            items,
            page,
            pageSize,
            totalItems,
            totalPages,
            page < totalPages,
            page > 1);
    }
}