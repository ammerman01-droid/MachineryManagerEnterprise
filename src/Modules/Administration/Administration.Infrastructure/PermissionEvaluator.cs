using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Administration.Infrastructure;

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
    private readonly IOrganizationLookupService _organizationLookup;
    private readonly IProjectLookupService _projectLookup;

    /// <summary>Initializes a new instance of the <see cref="PermissionEvaluator"/> class.</summary>
    /// <param name="dbContext">The Administration module's persistence context.</param>
    /// <param name="organizationLookup">Resolves an Organization's Holding.</param>
    /// <param name="projectLookup">Resolves a Project's Organization.</param>
    public PermissionEvaluator(
        Persistence.AdministrationDbContext dbContext,
        IOrganizationLookupService organizationLookup,
        IProjectLookupService projectLookup)
    {
        _dbContext = dbContext;
        _organizationLookup = organizationLookup;
        _projectLookup = projectLookup;
    }

    /// <inheritdoc />
    public async Task<bool> HasPermissionAsync(
        Guid userId,
        string permission,
        ResourceScope resourceScope,
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

        foreach (var assignment in assignments)
        {
            var hasPermission = assignment.Permissions.Contains(WildcardPermission)
                || assignment.Permissions.Contains(permission);

            if (!hasPermission)
            {
                continue;
            }

            if (Covers(assignment.Scope, resourceScope)
                || await CoversHoldingReferenceDataAsync(assignment.Scope, permission, resourceScope, cancellationToken))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// The one exception to the strictly top-down scope rule: Holding-level reference data (see
    /// <see cref="global::Administration.Domain.HoldingReferenceData"/>) of the user's OWN Holding is
    /// covered by Organization-level assignments (every action the profile grants) and by
    /// Project-level assignments (view only, whatever the profile contains).
    /// </summary>
    private async Task<bool> CoversHoldingReferenceDataAsync(
        global::Administration.Domain.AuthorizationScope assignmentScope,
        string permission,
        ResourceScope resource,
        CancellationToken cancellationToken)
    {
        if (resource.HoldingId is not { } holdingId
            || resource.OrganizationId is not null
            || resource.ProjectId is not null
            || !global::Administration.Domain.HoldingReferenceData.TryGetAction(permission, out var action))
        {
            return false;
        }

        switch (assignmentScope.Level)
        {
            case global::Administration.Domain.AuthorizationScopeLevel.Organization
                when assignmentScope.OrganizationId is { } organizationId:
                return await _organizationLookup.GetHoldingIdAsync(organizationId, cancellationToken) == holdingId;

            case global::Administration.Domain.AuthorizationScopeLevel.Project
                when assignmentScope.ProjectId is { } projectId
                    && action == global::Administration.Domain.HoldingReferenceData.ReadOnlyAction:
                var parentOrganizationId = await _projectLookup.GetOrganizationIdAsync(projectId, cancellationToken);

                return parentOrganizationId is { } parent
                    && await _organizationLookup.GetHoldingIdAsync(parent, cancellationToken) == holdingId;

            default:
                return false;
        }
    }


    /// <inheritdoc />
    public async Task<AuthorizedScopeSet> GetAuthorizedScopesAsync(
        Guid userId,
        string permission,
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

        var holdingIds = new HashSet<Guid>();
        var organizationIds = new HashSet<Guid>();
        var projectIds = new HashSet<Guid>();

        foreach (var assignment in assignments)
        {
            var hasPermission = assignment.Permissions.Contains(WildcardPermission)
                || assignment.Permissions.Contains(permission);

            if (!hasPermission)
            {
                continue;
            }

            switch (assignment.Scope.Level)
            {
                case global::Administration.Domain.AuthorizationScopeLevel.Platform:
                    return AuthorizedScopeSet.Unrestricted;

                case global::Administration.Domain.AuthorizationScopeLevel.Holding
                    when assignment.Scope.HoldingId is { } holdingId:
                    holdingIds.Add(holdingId);
                    break;

                case global::Administration.Domain.AuthorizationScopeLevel.Organization
                    when assignment.Scope.OrganizationId is { } organizationId:
                    organizationIds.Add(organizationId);
                    break;

                case global::Administration.Domain.AuthorizationScopeLevel.Project
                    when assignment.Scope.ProjectId is { } projectId:
                    projectIds.Add(projectId);
                    break;
            }
        }

        if (holdingIds.Count == 0 && organizationIds.Count == 0 && projectIds.Count == 0)
        {
            return AuthorizedScopeSet.None;
        }

        return new AuthorizedScopeSet(false, holdingIds, organizationIds, projectIds);
    }
    internal static bool Covers(global::Administration.Domain.AuthorizationScope assignmentScope, ResourceScope resource)
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
