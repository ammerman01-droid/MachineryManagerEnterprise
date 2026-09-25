namespace MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence;

/// <summary>
/// Infrastructure-only persistence record backing the per-Organization
/// sequential Work Order numbering (chat, 2026-09-22 — confirmed
/// necessary, per-Organization starting at 1). NOT a Domain entity or
/// aggregate — it carries no business behavior, exists solely so
/// <see cref="WorkOrderNumberGenerator"/> has a row to atomically
/// increment under concurrency.
/// </summary>
public sealed class WorkOrderSequence
{
    /// <summary>Gets or sets the Organization this counter belongs to (primary key).</summary>
    public Guid OrganizationId { get; set; }

    /// <summary>Gets or sets the last Work Order number issued for this Organization.</summary>
    public int LastNumber { get; set; }
}
