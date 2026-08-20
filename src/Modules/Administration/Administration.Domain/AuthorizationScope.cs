using MachineryManager.SharedKernel;

namespace Administration.Domain;

/// <summary>
/// Represents the tenant scope at which a Profile is assigned to a User (Section 5.8).
/// </summary>
public sealed class AuthorizationScope : ValueObject
{
    /// <summary>Gets the level of the scope.</summary>
    public AuthorizationScopeLevel Level { get; private set; }

    /// <summary>Gets the Holding identifier when applicable.</summary>
    public Guid? HoldingId { get; private set; }

    /// <summary>Gets the Organization identifier when applicable.</summary>
    public Guid? OrganizationId { get; private set; }

    /// <summary>Gets the Project identifier when applicable.</summary>
    public Guid? ProjectId { get; private set; }

    // Reserved for EF Core materialization only.
    private AuthorizationScope()
    {
    }

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
    public static AuthorizationScope ForHolding(Guid holdingId) =>
        new(AuthorizationScopeLevel.Holding, holdingId, null, null);

    /// <summary>Creates an organization-level scope.</summary>
    public static AuthorizationScope ForOrganization(Guid organizationId) =>
        new(AuthorizationScopeLevel.Organization, null, organizationId, null);

    /// <summary>Creates a project-level scope.</summary>
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