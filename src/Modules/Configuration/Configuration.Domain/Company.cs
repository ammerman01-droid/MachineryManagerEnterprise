using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a manufacturer Company available within a Holding.
/// A Company is master/reference data used by Asset Models and Engine Models.
/// </summary>
public sealed class Company : AggregateRoot<CompanyId>
{
    /// <summary>The maximum allowed length for a Company name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>Gets the identifier of the Holding that owns this Company.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the Company's display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets whether this Company is currently active. A deactivated
    /// (soft-deleted) Company is kept in the database for historical
    /// and audit purposes, but is excluded from selection lists by
    /// default (chat, 2026-09-19).
    /// </summary>
    public bool IsActive { get; private set; } = true;

    private Company()
    {
    }

    private Company(CompanyId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

    /// <summary>
    /// Registers a new Company in the specified Holding.
    /// </summary>
    /// <param name="holdingId">The owning Holding.</param>
    /// <param name="name">The Company's display name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>The newly created Company or a validation failure.</returns>
    public static Result<Company> Register(
        Guid holdingId,
        string name,
        IDateTimeProvider dateTimeProvider)
    {
        if (holdingId == Guid.Empty)
        {
            return Result.Failure<Company>(
                CompanyErrors.NotFound(holdingId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Company>(
                CompanyErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Company>(
                CompanyErrors.NameTooLong(MaxNameLength));
        }

        var company = new Company(
            CompanyId.New(),
            holdingId,
            name.Trim());

        company.RaiseDomainEvent(
            new CompanyRegistered(
                company.Id,
                holdingId,
                company.Name,
                dateTimeProvider.UtcNow));

        return company;
    }

    /// <summary>
    /// Updates the Company's display name.
    /// </summary>
    /// <param name="name">The new display name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a validation failure.</returns>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(CompanyErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(CompanyErrors.NameTooLong(MaxNameLength));
        }

        var trimmed = name.Trim();

        if (trimmed == Name)
        {
            return Result.Success();
        }

        Name = trimmed;

        RaiseDomainEvent(new CompanyUpdated(Id, HoldingId, Name, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Deactivates the Company (soft delete). A deactivated Company is
    /// excluded from selection lists but remains in the database.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a failure if the Company is already inactive.</returns>
    public Result Deactivate(IDateTimeProvider dateTimeProvider)
    {
        if (!IsActive)
        {
            return Result.Failure(CompanyErrors.AlreadyInactive());
        }

        IsActive = false;

        RaiseDomainEvent(new CompanyDeactivated(Id, HoldingId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Reactivates a previously deactivated Company.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a failure if the Company is already active.</returns>
    public Result Activate(IDateTimeProvider dateTimeProvider)
    {
        if (IsActive)
        {
            return Result.Failure(CompanyErrors.AlreadyActive());
        }

        IsActive = true;

        RaiseDomainEvent(new CompanyActivated(Id, HoldingId, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}
