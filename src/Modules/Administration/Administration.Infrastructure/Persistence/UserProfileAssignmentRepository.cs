using MachineryManager.Administration.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Administration.Domain;

namespace MachineryManager.Administration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IUserProfileAssignmentRepository"/>.</summary>
public sealed class UserProfileAssignmentRepository : IUserProfileAssignmentRepository
{
    private readonly AdministrationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UserProfileAssignmentRepository"/> class.</summary>
    /// <param name="dbContext">The Administration module's persistence context.</param>
    public UserProfileAssignmentRepository(AdministrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<UserProfileAssignment?> GetByIdAsync(UserProfileAssignmentId id, CancellationToken cancellationToken = default) =>
        _dbContext.UserProfileAssignments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(UserProfileAssignment aggregate) => _dbContext.UserProfileAssignments.Add(aggregate);

    /// <inheritdoc />
    public void Update(UserProfileAssignment aggregate) => _dbContext.UserProfileAssignments.Update(aggregate);

    /// <inheritdoc />
    public void Remove(UserProfileAssignment aggregate) => _dbContext.UserProfileAssignments.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserProfileAssignment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await _dbContext.UserProfileAssignments
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserProfileAssignment>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await _dbContext.UserProfileAssignments
            .Where(a => a.UserId == userId && a.IsActive)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public Task<bool> HasActiveAssignmentsForProfileAsync(ProfileId profileId, CancellationToken cancellationToken = default) =>
        _dbContext.UserProfileAssignments
            .AnyAsync(a => a.ProfileId == profileId && a.IsActive, cancellationToken);

    /// <inheritdoc />
    public async Task RemoveAllForProfileAsync(ProfileId profileId, CancellationToken cancellationToken = default)
    {
        var assignments = await _dbContext.UserProfileAssignments
            .Where(a => a.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        _dbContext.UserProfileAssignments.RemoveRange(assignments);
    }
}