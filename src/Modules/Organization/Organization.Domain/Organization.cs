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

    /// <summary>
    /// Gets whether this Organization is currently suspended (BR-017,
    /// Section 10.16, RESOLVED). Suspension never deletes or hides
    /// historical records — it is purely a status flag consumed by
    /// callers as needed. Full lifecycle states beyond Suspension
    /// (e.g. permanent closure) remain an open question and are
    /// intentionally NOT modeled here.
    /// </summary>
    public bool IsSuspended { get; private set; }

    /// <summary>Gets the UTC timestamp when the organization was suspended, or null if not suspended.</summary>
    public DateTimeOffset? SuspendedAt { get; private set; }

    // Reserved for EF Core materialization only.
    private Organization()
    {
        Name = string.Empty;
    }

    private Organization(OrganizationId id, string name)
        : base(id)
    {
        Name = name;
        IsSuspended = false;
        SuspendedAt = null;
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

    
    /// <summary>Renames this Organization.</summary>
    /// <param name="name">The new name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(OrganizationErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(OrganizationErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();

        RaiseDomainEvent(new OrganizationRenamed(Id, Name, dateTimeProvider.UtcNow));

        return Result.Success();
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

    /// <summary>
    /// Suspends this Organization (BR-017, Section 10.16, RESOLVED).
    /// Historical records are never deleted or hidden by suspension —
    /// this method only flips the status flag and raises the
    /// corresponding domain event; any downstream effects (if ever
    /// decided) belong to their own Bounded Contexts.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a conflict error if already suspended.</returns>
    public Result Suspend(IDateTimeProvider dateTimeProvider)
    {
        if (IsSuspended)
        {
            return Result.Failure(OrganizationErrors.AlreadySuspended());
        }

        IsSuspended = true;
        SuspendedAt = dateTimeProvider.UtcNow;

        RaiseDomainEvent(new OrganizationSuspended(Id, SuspendedAt.Value));

        return Result.Success();
    }

    /// <summary>
    /// Reactivates a previously suspended Organization.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a conflict error if not currently suspended.</returns>
    public Result Reactivate(IDateTimeProvider dateTimeProvider)
    {
        if (!IsSuspended)
        {
            return Result.Failure(OrganizationErrors.NotSuspended());
        }

        IsSuspended = false;
        SuspendedAt = null;

        RaiseDomainEvent(new OrganizationReactivated(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}