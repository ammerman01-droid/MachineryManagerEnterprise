using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Organization.Domain.Events;

namespace Organization.Domain;

/// <summary>
/// Organization (GL-ORG-001): the tenant boundary and business owner
/// of Assets (via Project), per BR-017 (Business Specification —
/// Organization Management).
/// </summary>
public sealed class Organization : AggregateRoot<OrganizationId>
{
    /// <summary>The maximum allowed length for an organization name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>Gets the name of the organization.</summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the identifier of the Holding this Organization belongs
    /// to, if any (chat, 2026-08-19). An Organization may exist
    /// without a Holding (standalone tenant).
    /// </summary>
    public HoldingId? HoldingId { get; private set; }

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

    /// <summary>
    /// Assigns this Organization to a Holding (chat, 2026-08-19). An
    /// Organization belongs to at most one Holding at a time.
    /// </summary>
    /// <param name="holdingId">The identifier of the Holding.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result AssignToHolding(HoldingId holdingId, IDateTimeProvider dateTimeProvider)
    {
        if (holdingId is null)
        {
            return Result.Failure(OrganizationErrors.HoldingRequired());
        }

        HoldingId = holdingId;

        RaiseDomainEvent(new OrganizationAssignedToHolding(Id, holdingId, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}