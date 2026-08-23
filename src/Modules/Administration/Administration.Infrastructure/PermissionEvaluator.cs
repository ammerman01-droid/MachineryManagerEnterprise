using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Administration.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IPermissionEvaluator"/>.
/// Queries the current state of <c>UserProfileAssignment</c> at
/// request time — no caching (chat, 2026-08-22).
/// </summary>
/// <remarks>
/// Convention: a Profile whose Permissions list contains "*" grants
/// every permission within that assignment's scope (this is how a
/// SuperUser assignment is represented — Section 5.8 does not define
/// a separate boolean flag for it).
/// </remarks>
public sealed class PermissionEvaluator : IPermissionEvaluator
{
    private const string WildcardPermission = "*";

    private readonly Persistence.AdministrationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="PermissionEvaluator"/> class.</summary>
    /// <param name="dbContext">The Administration module's persistence context.</param>
    public PermissionEvaluator(Persistence.AdministrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<bool> HasPermissionAsync(
        Guid userId,
        string permission,
        ResourceScope resourceScope,
        CancellationToken cancellationToken = default)
    {
        var assignments = await _dbContext.Set<global::Administration.Domain.UserProfileAssignment>()
            .Where(a => a.UserId == userId)
            .Join(
                _dbContext.Set<global::Administration.Domain.Profile>(),
                assignment => assignment.ProfileId,
                profile => profile.Id,
                (assignment, profile) => new { assignment.Scope, profile.Permissions, profile.IsActive })
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var assignment in assignments)
        {
            var hasPermission = assignment.Permissions.Contains(WildcardPermission)
                || assignment.Permissions.Contains(permission);

            if (!hasPermission)
            {
                continue;
            }

            if (Covers(assignment.Scope, resourceScope))
            {
                return true;
            }
        }

        return false;
    }

    private static bool Covers(global::Administration.Domain.AuthorizationScope assignmentScope, ResourceScope resource)
    {
        return assignmentScope.Level switch
        {
            global::Administration.Domain.AuthorizationScopeLevel.Platform => true,
            global::Administration.Domain.AuthorizationScopeLevel.Holding =>
                resource.HoldingId is not null && assignmentScope.HoldingId == resource.HoldingId,
            global::Administration.Domain.AuthorizationScopeLevel.Organization =>
                resource.OrganizationId is not null && assignmentScope.OrganizationId == resource.OrganizationId,
            global::Administration.Domain.AuthorizationScopeLevel.Project =>
                resource.ProjectId is not null && assignmentScope.ProjectId == resource.ProjectId,
            _ => false,
        };
    }
}