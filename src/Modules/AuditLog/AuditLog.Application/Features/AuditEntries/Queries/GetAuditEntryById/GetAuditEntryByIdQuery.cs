using MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.GetAuditEntryById;

/// <summary>
/// Retrieves a single audit record with its field-level changes
/// (chat, 2026-09-06, gam 6).
/// </summary>
/// <param name="AuditEntryId">The audit record's identifier.</param>
public sealed record GetAuditEntryByIdQuery(Guid AuditEntryId)
    : IRequest<Result<AuditEntryDetailDto>>;