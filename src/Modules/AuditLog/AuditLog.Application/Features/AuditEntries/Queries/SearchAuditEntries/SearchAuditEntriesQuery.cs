using MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.SearchAuditEntries;

/// <summary>
/// Searches the platform-wide audit trail with optional filters on
/// date range, user, operation type, and source table
/// (chat, 2026-09-05, gam 4).
/// </summary>
/// <param name="From">Optional lower bound (inclusive) on the change timestamp.</param>
/// <param name="To">Optional upper bound (inclusive) on the change timestamp.</param>
/// <param name="UserId">Optional filter: only changes made by this user.</param>
/// <param name="OperationType">Optional filter: only this kind of change.</param>
/// <param name="SchemaName">Optional exact filter on the source schema name.</param>
/// <param name="TableName">Optional partial filter on the source table name.</param>
/// <param name="Page">One-based page number.</param>
/// <param name="PageSize">Number of records per page.</param>
public sealed record SearchAuditEntriesQuery(
    DateTimeOffset? From,
    DateTimeOffset? To,
    Guid? UserId,
    AuditOperationType? OperationType,
    string? SchemaName,
    string? TableName,
    int Page,
    int PageSize) : IRequest<Result<AuditEntrySearchResponse>>;