using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.Administration.Application.Features.Profiles.Dtos;
using MachineryManager.Administration.Application.Features.Profiles.Queries.SearchProfiles;
using Microsoft.EntityFrameworkCore;
using Administration.Domain;

namespace MachineryManager.Administration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IProfileRepository"/>.</summary>
public sealed class ProfileRepository : IProfileRepository
{
    private readonly AdministrationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="ProfileRepository"/> class.</summary>
    /// <param name="dbContext">The Administration module's persistence context.</param>

    public ProfileRepository(AdministrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<global::Administration.Domain.Profile?> GetByIdAsync(ProfileId id, CancellationToken cancellationToken = default) =>
        _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(global::Administration.Domain.Profile aggregate) => _dbContext.Profiles.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::Administration.Domain.Profile aggregate) => _dbContext.Profiles.Update(aggregate);

    /// <inheritdoc />
    public async Task<SearchProfilesResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Profiles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProfileDto(
                p.Id.Value,
                p.Name,
                p.Permissions.ToList(),
                p.IsActive,
                p.CreatedAt))
            .ToListAsync(cancellationToken);

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchProfilesResponse(
            items,
            page,
            pageSize,
            totalItems,
            totalPages,
            page < totalPages,
            page > 1);
    }
}