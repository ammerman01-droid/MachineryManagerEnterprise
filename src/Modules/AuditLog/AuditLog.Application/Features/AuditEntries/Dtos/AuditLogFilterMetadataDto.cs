namespace MachineryManagerEnterprise.AuditLog.Application.Features.AuditEntries.Dtos;

/// <summary>
/// The distinct filter values currently present in the audit trail,
/// used to populate the search filters of the AuditLog list screen
/// (chat, 2026-09-07, gam 6 — searchable filter selects).
/// </summary>
/// <param name="SchemaNames">Distinct schema names, ordered alphabetically.</param>
/// <param name="TableNames">Distinct table names, ordered alphabetically.</param>
public sealed record AuditLogFilterMetadataDto(
    IReadOnlyList<string> SchemaNames,
    IReadOnlyList<string> TableNames);