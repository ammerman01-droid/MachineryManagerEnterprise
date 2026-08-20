using MachineryManager.SharedKernel;

namespace Administration.Domain;

/// <summary>
/// Represents the tenant scope at which a Profile is assigned to a
/// User (Section 5.8). Immutable value object.
/// </summary>
public sealed class AuthorizationScope : ValueObject
{
    /// <summary>Gets the level of the scope.</summary>
    public AuthorizationScopeLevel Level { get; }

    /// <summary>Gets the Holding identifier when <see cref="Level"/> is <see cref="AuthorizationScopeLevel.Holding"/>.</summary>
    public Guid? HoldingId { get; }

    /// <summary>Gets the Organization identifier when <see cref="Level"/> is <see cref="AuthorizationScopeLevel.Organization"/> or below.</summary>
    public Guid? OrganizationId { get; }

    /// <summary>Gets the Project identifier when <see cref="Level"/> is <see cref="AuthorizationScopeLevel.Project"/>.</summary>
    public Guid? ProjectId { get; }

    private AuthorizationScope(
        AuthorizationScopeLevel level,
        Guid? holdingId,
        Guid? organizationId,
        Guid? projectId)
    {
        Level = level;
        HoldingId = holdingId;
        OrganizationId = organizationId;
        ProjectId = projectId;
    }

    /// <summary>Creates a platform-level scope.</summary>
    public static AuthorizationScope Platform() =>
        new(AuthorizationScopeLevel.Platform, null, null, null);

    /// <summary>Creates a holding-level scope.</summary>
    /// <param name="holdingId">The holding identifier.</param>
    public static AuthorizationScope ForHolding(Guid holdingId) =>
        new(AuthorizationScopeLevel.Holding, holdingId, null, null);

    /// <summary>Creates an organization-level scope.</summary>
    /// <param name="organizationId">The organization identifier.</param>
    public static AuthorizationScope ForOrganization(Guid organizationId) =>
        new(AuthorizationScopeLevel.Organization, null, organizationId, null);

    /// <summary>Creates a project-level scope.</summary>
    /// <param name="projectId">The project identifier.</param>
    public static AuthorizationScope ForProject(Guid projectId) =>
        new(AuthorizationScopeLevel.Project, null, null, projectId);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Level;
        yield return HoldingId;
        yield return OrganizationId;
        yield return ProjectId;
    }
}