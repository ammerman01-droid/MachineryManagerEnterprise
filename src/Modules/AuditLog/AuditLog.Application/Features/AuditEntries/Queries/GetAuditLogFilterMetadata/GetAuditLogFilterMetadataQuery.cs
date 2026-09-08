using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.GetAuditLogFilterMetadata;

/// <summary>
/// Retrieves the distinct schema and table names visible to the current user
/// for populating the Audit Log filter controls.
/// </summary>
public sealed record GetAuditLogFilterMetadataQuery
    : IRequest<Result<FilterMetadataResult>>;

/// <summary>
/// Contains the metadata used by the Audit Log filtering controls.
/// </summary>
/// <param name="SchemaNames">Distinct database schema names visible to the caller.</param>
/// <param name="TableNames">Distinct database table names visible to the caller.</param>
public sealed record FilterMetadataResult(
    IReadOnlyList<string> SchemaNames,
    IReadOnlyList<string> TableNames);