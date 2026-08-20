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
}