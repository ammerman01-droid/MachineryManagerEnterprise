using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a Company is deactivated (soft-deleted).</summary>
public sealed class CompanyDeactivated : IDomainEvent
{
    /// <summary>Gets the CompanyId value.</summary>
    public CompanyId CompanyId { get; }

    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="CompanyDeactivated"/> class.</summary>
    public CompanyDeactivated(CompanyId companyId, Guid holdingId, DateTimeOffset occurredOn)
    {
        CompanyId = companyId;
        HoldingId = holdingId;
        OccurredOn = occurredOn;
    }
}
