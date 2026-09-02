using Configuration.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

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
}