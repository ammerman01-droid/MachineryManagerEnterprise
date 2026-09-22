using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Company's display name is updated.</summary>
public sealed class CompanyUpdated : IDomainEvent
{
    /// <summary>Gets the CompanyId value.</summary>
    public CompanyId CompanyId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the new Name value.</summary>
    public string Name { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="CompanyUpdated"/> class.</summary>
    public CompanyUpdated(CompanyId companyId, Guid holdingId, string name, DateTimeOffset occurredOn)
    {
        CompanyId = companyId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}
