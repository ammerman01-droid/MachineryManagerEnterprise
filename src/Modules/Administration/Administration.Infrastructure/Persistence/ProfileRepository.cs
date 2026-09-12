using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Dtos;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.SearchProfiles;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Administration.Domain;

namespace MachineryManagerEnterprise.Administration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IProfileRepository"/>.</summary>
public sealed class ProfileRepository : IProfileRepository
{
    private readonly AdministrationDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="ProfileRepository"/> class.</summary>
    /// <param name="dbContext">The Administration module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public ProfileRepository(AdministrationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<global::Administration.Domain.Profile?> GetByIdAsync(ProfileId id, CancellationToken cancellationToken = default) =>
        _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(global::Administration.Domain.Profile aggregate) => _dbContext.Profiles.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::Administration.Domain.Profile aggregate) => _dbContext.Profiles.Update(aggregate);

    /// <inheritdoc />
    public void Remove(global::Administration.Domain.Profile aggregate) => _dbContext.Profiles.Remove(aggregate);

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

        // Materialize the Profile entities first (pagination still runs
        // in SQL via OrderBy/Skip/Take), then map to ProfileDto in
        // memory via Mapster's IMapper — NOT .ProjectToType() on the
        // IQueryable, which would regenerate the same Select()
        // expression that triggers EF Core 10's NullReferenceException
        // on the field-backed primitive collection Profile.Permissions
        // (chat, 2026-08-24).
        var entities = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = _mapper.Map<List<ProfileDto>>(entities);

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