namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;

/// <summary>
/// One field-level change of an audit record, parsed from the
/// <c>ChangesJson</c> payload written by AuditSaveChangesInterceptor
/// (chat, 2026-09-06, gam 6 — change detail view).
/// </summary>
/// <param name="Field">The name of the changed property.</param>
/// <param name="OldValue">The value before the change (null for Created entries and null-valued fields).</param>
/// <param name="NewValue">The value after the change (null for Deleted entries and null-valued fields).</param>
public sealed record AuditFieldChangeDto(
    string Field,
    string? OldValue,
    string? NewValue);