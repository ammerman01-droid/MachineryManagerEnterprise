using MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.SearchAuditEntries;
using MachineryManager.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.GetAuditEntryById;

namespace MachineryManager.AuditLog.Presentation.Endpoints;

/// <summary>
/// Maps the AuditLog module's REST endpoints per 07-api conventions
/// (Section 8): base path <c>/api/v1/audit-logs</c>
/// (chat, 2026-09-05, gam 4).
/// </summary>
/// <remarks>
/// gam 5 will replace the plain RequireAuthenticatedUser with a
/// dedicated permission from the Administration module's
/// PermissionCatalog plus HoldingId/OrganizationId scope filtering.
/// Authenticated users only at the transport layer; the
/// AuditLog.View permission and HoldingId/OrganizationId scope filtering
/// are enforced inside <c>SearchAuditEntriesQueryHandler</c>
/// (chat, 2026-09-06, gam 5).
/// </remarks>
public static class AuditLogEndpoints
{
    /// <summary>Registers the AuditLog endpoints on the application's route builder.</summary>
    public static IEndpointRouteBuilder MapAuditLogEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/audit-logs")
            .WithTags("AuditLog")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapGet("/", SearchAuditEntriesAsync)
            .WithName("SearchAuditEntries")
            .WithSummary("Searches the platform-wide audit trail, with optional filters on date range, user, operation type, and source table.");

        group.MapGet("/{auditEntryId:guid}", GetAuditEntryByIdAsync)
            .WithName("GetAuditEntryById")
            .WithSummary("Retrieves a single audit record, including its parsed field-level changes.");

        return endpoints;
    }

    private static async Task<IResult> SearchAuditEntriesAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        Guid? userId = null,
        AuditOperationType? operationType = null,
        string? schemaName = null,
        string? tableName = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchAuditEntriesQuery(from, to, userId, operationType, schemaName, tableName, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

        private static async Task<IResult> GetAuditEntryByIdAsync(
        Guid auditEntryId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAuditEntryByIdQuery(auditEntryId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }
}