using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Holdings.Dtos;
using MachineryManager.Organization.Application.Features.Holdings.Queries.SearchHoldings;
using Microsoft.EntityFrameworkCore;
using Organization.Domain;

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
    public async Task<SearchHoldingsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Holdings.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(h => h.Name.Contains(searchTerm));
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