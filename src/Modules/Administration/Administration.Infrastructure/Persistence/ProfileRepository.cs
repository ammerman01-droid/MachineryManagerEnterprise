using MachineryManager.Administration.Application.Abstractions;
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
}