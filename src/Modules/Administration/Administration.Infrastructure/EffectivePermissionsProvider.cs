using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Administration.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IEffectivePermissionsProvider"/>: one query
/// for the user's active assignments with active profiles, then the same scope-coverage
/// rule <see cref="PermissionEvaluator"/> uses, so both always agree.
/// </summary>
public sealed class EffectivePermissionsProvider : IEffectivePermissionsProvider
{
    private readonly Persistence.AdministrationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="EffectivePermissionsProvider"/> class.</summary>
    /// <param name="dbContext">The Administration module's persistence context.</param>
    public EffectivePermissionsProvider(Persistence.AdministrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<EffectivePermissions> GetAsync(
        Guid userId,
        ResourceScope scope,
        CancellationToken cancellationToken = default)
    {
        var assignments = await _dbContext.Set<global::Administration.Domain.UserProfileAssignment>()
            .Where(a => a.UserId == userId && a.IsActive)
            .Join(
                _dbContext.Set<global::Administration.Domain.Profile>(),
                assignment => assignment.ProfileId,
                profile => profile.Id,
                (assignment, profile) => new { assignment.Scope, profile.Permissions, profile.IsActive })
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        var hasWildcard = false;
        var permissions = new HashSet<string>(StringComparer.Ordinal);

        foreach (var assignment in assignments)
        {
            if (!PermissionEvaluator.Covers(assignment.Scope, scope))
            {
                continue;
            }

            foreach (var permission in assignment.Permissions)
            {
                if (permission == global::Administration.Domain.Profile.WildcardPermission)
                {
                    hasWildcard = true;
                }
                else
                {
                    permissions.Add(permission);
                }
            }
        }

        return hasWildcard || permissions.Count > 0
            ? new EffectivePermissions(hasWildcard, permissions)
            : EffectivePermissions.None;
    }
}
