using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Organization.Domain.Events;

namespace Organization.Domain;

/// <summary>
/// Organization (GL-ORG-001): the tenant boundary and business owner
/// of Assets, per BR-017 (Business Specification — Organization
/// Management).
///
/// This Aggregate is intentionally minimal. BR-017 explicitly leaves
/// sub-organizations, ownership transfer, and any lifecycle beyond
/// registration as open questions (Section 9) — behavior for those
/// shall not be added here until Domain Discovery resolves them.
/// </summary>
public sealed class Organization : AggregateRoot<OrganizationId>
{
    /// <summary>The maximum allowed length for an organization name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>Gets the name of the organization.</summary>
    public string Name { get; private set; }

    // Reserved for EF Core materialization only.
    private Organization()
    {
        Name = string.Empty;
    }

    private Organization(OrganizationId id, string name)
        : base(id)
    {
        Name = name;
    }

    /// <summary>
    /// Registers a new Organization (UC-1301 / CMD-950). This is the
    /// only way an Organization comes into existence.
    /// </summary>
    public static Result<Organization> Register(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Organization>(OrganizationErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Organization>(OrganizationErrors.NameTooLong(MaxNameLength));
        }

        var organization = new Organization(OrganizationId.New(), name.Trim());

        organization.RaiseDomainEvent(
            new OrganizationRegistered(organization.Id, organization.Name, dateTimeProvider.UtcNow));

        return organization;
    }
}