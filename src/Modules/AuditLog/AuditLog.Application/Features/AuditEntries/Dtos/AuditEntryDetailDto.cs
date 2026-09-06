using MachineryManager.SharedKernel;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;

/// <summary>
/// Full projection of a single <see cref="AuditEntry"/>, including its
/// parsed field-level changes (chat, 2026-09-06, gam 6).
/// </summary>
public sealed record AuditEntryDetailDto(
    Guid Id,
    Guid? UserId,
    DateTimeOffset OccurredAt,
    string SchemaName,
    string TableName,
    string RecordId,
    AuditOperationType OperationType,
    Guid? HoldingId,
    Guid? OrganizationId,
    IReadOnlyList<AuditFieldChangeDto> Changes);