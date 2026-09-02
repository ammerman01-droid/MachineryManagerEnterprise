using MachineryManager.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>
/// Raised when a new Company is registered in a Holding.
/// </summary>
public sealed class CompanyRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered Company.</summary>
    public CompanyId CompanyId { get; }

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the registered Company's name.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyRegistered"/> class.
    /// </summary>
    /// <param name="companyId">The identifier of the registered Company.</param>
    /// <param name="holdingId">The owning Holding.</param>
    /// <param name="name">The Company name.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public CompanyRegistered(
        CompanyId companyId,
        Guid holdingId,
        string name,
        DateTimeOffset occurredOn)
    {
        CompanyId = companyId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}