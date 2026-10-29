using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Administration.Domain;

/// <summary>
/// A user's fixed position in the Holding/Organization/Project tree: the node where the
/// account was created. It defines who may see and manage the user (Section 5.8) and never
/// changes. A user without a row is a Platform user. Owned by Administration, not Identity,
/// because Identity never stores Organization data (ADR-0030).
/// </summary>
public sealed class UserHome
{
    /// <summary>Gets the identifier of the user this home belongs to.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Gets the Holding the user belongs to.</summary>
    public Guid? HoldingId { get; private set; }

    /// <summary>Gets the Organization, for Organization- and Project-level users.</summary>
    public Guid? OrganizationId { get; private set; }

    /// <summary>Gets the Project, for Project-level users.</summary>
    public Guid? ProjectId { get; private set; }

    /// <summary>Gets the UTC timestamp when the home was recorded.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>Gets the hierarchy level of this home.</summary>
    public ScopeLevel Level =>
        ProjectId is not null ? ScopeLevel.Project
        : OrganizationId is not null ? ScopeLevel.Organization
        : HoldingId is not null ? ScopeLevel.Holding
        : ScopeLevel.Platform;

    // Reserved for EF Core materialization only.
    private UserHome()
    {
    }

    private UserHome(Guid userId, Guid? holdingId, Guid? organizationId, Guid? projectId, DateTimeOffset createdAt)
    {
        UserId = userId;
        HoldingId = holdingId;
        OrganizationId = organizationId;
        ProjectId = projectId;
        CreatedAt = createdAt;
    }

    /// <summary>Creates the home of a user.</summary>
    /// <param name="userId">The user.</param>
    /// <param name="holdingId">The Holding.</param>
    /// <param name="organizationId">The Organization, if any.</param>
    /// <param name="projectId">The Project, if any.</param>
    /// <param name="createdAt">The UTC creation timestamp.</param>
    /// <returns>The new home.</returns>
    public static UserHome Create(
        Guid userId,
        Guid? holdingId,
        Guid? organizationId,
        Guid? projectId,
        DateTimeOffset createdAt) =>
        new(userId, holdingId, organizationId, projectId, createdAt);
}
