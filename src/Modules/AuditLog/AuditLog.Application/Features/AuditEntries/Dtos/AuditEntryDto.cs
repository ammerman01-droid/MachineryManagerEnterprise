using MachineryManager.SharedKernel;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;

/// <summary>
/// Flat list-item projection of a single <see cref="AuditEntry"/>
/// (chat, 2026-09-05, gam 4).
/// </summary>
/// <remarks>
/// Intentionally excludes <see cref="AuditEntry.ChangesJson"/> — field
/// payloads can be large, and the list screen (gam 6) shows only
/// metadata. A detail DTO including the payload will be added with the
/// detail view.
/// </remarks>
/// <param name="Id">The audit record's identifier.</param>
/// <param name="UserId">The user who made the change, if attributable.</param>
/// <param name="OccurredAt">The UTC timestamp of the change.</param>
/// <param name="SchemaName">The database schema of the changed table.</param>
/// <param name="TableName">The name of the changed table.</param>
/// <param name="RecordId">The primary key of the changed record, as a string.</param>
/// <param name="OperationType">The kind of change.</param>
/// <param name="HoldingId">The owning Holding's identifier, if resolvable.</param>
/// <param name="OrganizationId">The owning Organization's identifier, if resolvable.</param>
public sealed record AuditEntryDto(
    Guid Id,
    Guid? UserId,
    DateTimeOffset OccurredAt,
    string SchemaName,
    string TableName,
    string RecordId,
    AuditOperationType OperationType,
    Guid? HoldingId,
    Guid? OrganizationId);