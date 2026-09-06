using MachineryManager.AuditLog.Application.Abstractions;
using MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.SearchAuditEntries;

/// <summary>
/// Handles <see cref="SearchAuditEntriesQuery"/> by first resolving the
/// requesting user's authorized scopes for
/// <see cref="AuditLogPermissions.View"/>, then delegating the filtered,
/// paged read to <see cref="IAuditEntryReadRepository"/>
/// (chat, 2026-09-06, gam 5).
/// </summary>
/// <remarks>
/// <para>
/// This handler is the FIRST enforcement site of a permission in the
/// codebase (chat, 2026-09-06): per the PermissionCatalog convention,
/// each module's handlers check their permission string independently.
/// </para>
/// <para>
/// Deliberate decisions: (a) <see cref="AuthorizedScopeSet.None"/> and
/// project-only scopes return an EMPTY page rather than an error —
/// ErrorType has no Forbidden category; (b) the repository receives the
/// scope set and applies the (HoldingId ∈ set) OR (OrganizationId ∈ set)
/// predicate, because AuditEntry has no ProjectId column.
/// </para>
/// </remarks>
public sealed class SearchAuditEntriesQueryHandler
    : IRequestHandler<SearchAuditEntriesQuery, Result<AuditEntrySearchResponse>>
{
    private readonly IAuditEntryReadRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="SearchAuditEntriesQueryHandler"/> class.</summary>
    public SearchAuditEntriesQueryHandler(
        IAuditEntryReadRepository repository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <inheritdoc />
    public async Task<Result<AuditEntrySearchResponse>> Handle(
        SearchAuditEntriesQuery query,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Result.Failure<AuditEntrySearchResponse>(Error.Validation(
                "AuditLog.Unauthenticated",
                "The current request is not attributable to an authenticated user."));
        }

        var authorizedScope = await _permissionEvaluator.GetAuthorizedScopesAsync(
            userId.Value,
            AuditLogPermissions.View,
            cancellationToken);

        // gam 5: a user holding the permission only at Project level can
        // never match an AuditEntry (no ProjectId column) — same visible
        // outcome as holding no permission: an empty page.
        if (!authorizedScope.IsUnrestricted &&
            authorizedScope.HoldingIds.Count == 0 &&
            authorizedScope.OrganizationIds.Count == 0)
        {
            return new AuditEntrySearchResponse([], query.Page, query.PageSize, 0);
        }

        // null = Platform-level assignment: no scope filtering at all.
        var scope = authorizedScope.IsUnrestricted ? null : authorizedScope;

        var filter = new AuditEntrySearchFilter(
            query.From,
            query.To,
            query.UserId,
            query.OperationType,
            query.SchemaName,
            query.TableName,
            query.Page,
            query.PageSize,
            scope);

        var (items, totalCount) = await _repository.SearchAsync(filter, cancellationToken);

        var dtos = items
            .Select(entry => new AuditEntryDto(
                entry.Id,
                entry.UserId,
                entry.OccurredAt,
                entry.SchemaName,
                entry.TableName,
                entry.RecordId,
                entry.OperationType,
                entry.HoldingId,
                entry.OrganizationId))
            .ToList();

        return new AuditEntrySearchResponse(dtos, query.Page, query.PageSize, totalCount);
    }
}