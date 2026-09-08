using MachineryManager.AuditLog.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.GetAuditLogFilterMetadata;

/// <summary>
/// Handles <see cref="GetAuditLogFilterMetadataQuery"/> by retrieving
/// schema and table metadata restricted to the caller's authorized Audit Log scope.
/// </summary>
public sealed class GetAuditLogFilterMetadataQueryHandler
    : IRequestHandler<
        GetAuditLogFilterMetadataQuery,
        Result<FilterMetadataResult>>
{
    private const string AuditLogViewPermission = "AuditLog.View";

    private readonly IAuditEntryReadRepository _auditEntryReadRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetAuditLogFilterMetadataQueryHandler"/> class.
    /// </summary>
    /// <param name="auditEntryReadRepository">
    /// The read-only Audit Log repository.
    /// </param>
    /// <param name="currentUserService">
    /// Provides information about the currently authenticated user.
    /// </param>
    /// <param name="permissionEvaluator">
    /// Evaluates the user's Audit Log permission and authorized scopes.
    /// </param>
    public GetAuditLogFilterMetadataQueryHandler(
        IAuditEntryReadRepository auditEntryReadRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _auditEntryReadRepository = auditEntryReadRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>
    /// Retrieves the schema and table names visible to the current user.
    /// </summary>
    /// <param name="request">The metadata query.</param>
    /// <param name="cancellationToken">
    /// Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// The visible schema and table names, or an authorization error.
    /// </returns>
    public async Task<Result<FilterMetadataResult>> Handle(
        GetAuditLogFilterMetadataQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated ||
            !_currentUserService.UserId.HasValue)
        {
            return Result.Failure<FilterMetadataResult>(
                Error.Failure(
                    "AuditLog.Unauthorized",
                    "The current user is not authenticated."));
        }

        var userId = _currentUserService.UserId.Value;

        var authorizedScope =
            await _permissionEvaluator.GetAuthorizedScopesAsync(
                userId,
                AuditLogViewPermission,
                cancellationToken);

        if (authorizedScope == AuthorizedScopeSet.None)
        {
            return Result.Failure<FilterMetadataResult>(
                Error.Failure(
                    "AuditLog.Forbidden",
                    "The current user is not authorized to view the Audit Log."));
        }

        var schemaNames =
            await _auditEntryReadRepository.GetDistinctSchemaNamesAsync(
                authorizedScope,
                cancellationToken);

        var tableNames =
            await _auditEntryReadRepository.GetDistinctTableNamesAsync(
                authorizedScope,
                cancellationToken);

        return Result.Success(
            new FilterMetadataResult(schemaNames, tableNames));
    }
}