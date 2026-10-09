using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Administration.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IUserAuthorizationScopeResolver"/>.
/// The user's ACTIVE UserProfileAssignment carries the current scope;
/// AuthorizationScope deliberately stores only the ID at its own level,
/// so the parent chain (Project → Organization → Holding) is resolved
/// here through the Organization module's lookup abstractions.
/// </summary>
public sealed class UserAuthorizationScopeResolver : IUserAuthorizationScopeResolver
{
    private readonly Persistence.AdministrationDbContext _dbContext;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IProjectLookupService _projectLookupService;

    /// <summary>Initializes a new instance of the <see cref="UserAuthorizationScopeResolver"/> class.</summary>
    public UserAuthorizationScopeResolver(
        Persistence.AdministrationDbContext dbContext,
        IOrganizationLookupService organizationLookupService,
        IProjectLookupService projectLookupService)
    {
        _dbContext = dbContext;
        _organizationLookupService = organizationLookupService;
        _projectLookupService = projectLookupService;
    }

    /// <inheritdoc />
    public async Task<ResolvedUserScope?> ResolveAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var active = await _dbContext.UserProfileAssignments
            .AsNoTracking()
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderByDescending(a => a.LastChangedAt ?? a.AssignedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (active is null)
        {
            return null;
        }

        var holdingId = active.Scope.HoldingId;
        var organizationId = active.Scope.OrganizationId;
        var projectId = active.Scope.ProjectId;

        // Backfill the parent chain so consumers always receive the
        // full hierarchy for scoped users (Platform-level users keep
        // every member null — that null IS the platform semantics).
        if (projectId is { } pid && organizationId is null)
        {
            organizationId = await _projectLookupService.GetOrganizationIdAsync(pid, cancellationToken);
        }

        if (organizationId is { } oid && holdingId is null)
        {
            holdingId = await _organizationLookupService.GetHoldingIdAsync(oid, cancellationToken);
        }

        return new ResolvedUserScope(holdingId, organizationId, projectId);
    }
}