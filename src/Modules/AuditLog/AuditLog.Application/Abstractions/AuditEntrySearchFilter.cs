using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.AuditLog.Application.Abstractions;

/// <summary>
/// Immutable filter describing one page of audit records to retrieve
/// from the shared <c>audit.AuditEntry</c> table (chat, 2026-09-05,
/// gam 4 — read-only AuditLog module; scope added gam 5).
/// </summary>
/// <param name="From">Optional lower bound (inclusive) on <see cref="AuditEntry.OccurredAt"/>.</param>
/// <param name="To">Optional upper bound (inclusive) on <see cref="AuditEntry.OccurredAt"/>.</param>
/// <param name="UserId">Optional filter: only changes made by this user.</param>
/// <param name="OperationType">Optional filter: only this kind of change.</param>
/// <param name="SchemaName">Optional exact filter on the source schema name.</param>
/// <param name="TableName">Optional partial filter on the source table name.</param>
/// <param name="Page">One-based page number.</param>
/// <param name="PageSize">Number of records per page.</param>
/// <param name="AuthorizedScope">
/// The requesting user's authorized scopes for <see cref="AuditLogPermissions.View"/>,
/// or <c>null</c> when the user is unrestricted at platform level (no
/// scope filtering applied). Applied by the repository as
/// (HoldingId ∈ set) OR (OrganizationId ∈ set); project-level scopes are
/// not representable because AuditEntry has no ProjectId column.
/// </param>
public sealed record AuditEntrySearchFilter(
    DateTimeOffset? From,
    DateTimeOffset? To,
    Guid? UserId,
    AuditOperationType? OperationType,
    string? SchemaName,
    string? TableName,
    int Page,
    int PageSize,
    AuthorizedScopeSet? AuthorizedScope);