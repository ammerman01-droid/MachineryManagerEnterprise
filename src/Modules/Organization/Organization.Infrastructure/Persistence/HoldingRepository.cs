using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Holdings.Dtos;
using MachineryManager.Organization.Application.Features.Holdings.Queries.SearchHoldings;
using Microsoft.EntityFrameworkCore;
using Organization.Domain;
using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Organization.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IHoldingRepository"/>.
/// </summary>
public sealed class HoldingRepository : IHoldingRepository
{
    private readonly OrganizationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="HoldingRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The Organization module's persistence context.</param>
    public HoldingRepository(OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<Holding?> GetByIdAsync(HoldingId id, CancellationToken cancellationToken = default) =>
        _dbContext.Holdings.FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(Holding aggregate) => _dbContext.Holdings.Add(aggregate);

    /// <inheritdoc />
    public void Update(Holding aggregate) => _dbContext.Holdings.Update(aggregate);

    /// <inheritdoc />
    public void Remove(Holding aggregate) => _dbContext.Holdings.Remove(aggregate);

    /// <inheritdoc />
    public async Task<SearchHoldingsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        AuthorizedScopeSet authorizedScope,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Holdings.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(h => h.Name.Contains(searchTerm));
        }

        if (!authorizedScope.IsUnrestricted)
        {
            // Holding is the top of the tenant hierarchy (besides
            // Platform, which is Unrestricted) — a user sees a Holding
            // only if granted directly at Holding level (chat, 2026-08-23).
            var holdingIdSet = authorizedScope.HoldingIds
                .Select(HoldingId.From)
                .ToHashSet();

            query = query.Where(h => holdingIdSet.Contains(h.Id));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(h => h.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(h => new HoldingDto(h.Id.Value, h.Name))
            .ToListAsync(cancellationToken);

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchHoldingsResponse(
            items,
            page,
            pageSize,
            totalItems,
            totalPages,
            page < totalPages,
            page > 1);
    }
}