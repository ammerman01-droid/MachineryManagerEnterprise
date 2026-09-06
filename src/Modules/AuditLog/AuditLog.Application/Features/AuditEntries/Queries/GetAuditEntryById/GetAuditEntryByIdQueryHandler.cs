using System.Text.Json;
using MachineryManager.AuditLog.Application.Abstractions;
using MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.GetAuditEntryById;

/// <summary>
/// Handles <see cref="GetAuditEntryByIdQuery"/>: resolves the caller's
/// authorized scopes (same gam 5 rules as the search), fetches the
/// record, and parses its <c>ChangesJson</c> payload into typed changes
/// (chat, 2026-09-06, gam 6).
/// </summary>
/// <remarks>
/// Out-of-scope records deliberately return NotFound rather than 403, so
/// the endpoint does not reveal the existence of records the caller
/// cannot see (ErrorType has no Forbidden category).
/// </remarks>
public sealed class GetAuditEntryByIdQueryHandler
    : IRequestHandler<GetAuditEntryByIdQuery, Result<AuditEntryDetailDto>>
{
    private readonly IAuditEntryReadRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetAuditEntryByIdQueryHandler"/> class.</summary>
    public GetAuditEntryByIdQueryHandler(
        IAuditEntryReadRepository repository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <inheritdoc />
    public async Task<Result<AuditEntryDetailDto>> Handle(
        GetAuditEntryByIdQuery query,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Result.Failure<AuditEntryDetailDto>(Error.Validation(
                "AuditLog.Unauthenticated",
                "The current request is not attributable to an authenticated user."));
        }

        var entry = await _repository.GetByIdAsync(query.AuditEntryId, cancellationToken);

        if (entry is null)
        {
            return Result.Failure<AuditEntryDetailDto>(Error.NotFound(
                "AuditLog.NotFound",
                "The requested audit record does not exist."));
        }

        var authorizedScope = await _permissionEvaluator.GetAuthorizedScopesAsync(
            userId.Value,
            AuditLogPermissions.View,
            cancellationToken);

        if (!authorizedScope.IsUnrestricted &&
            !((entry.HoldingId.HasValue && authorizedScope.HoldingIds.Contains(entry.HoldingId.Value)) ||
              (entry.OrganizationId.HasValue && authorizedScope.OrganizationIds.Contains(entry.OrganizationId.Value))))
        {
            return Result.Failure<AuditEntryDetailDto>(Error.NotFound(
                "AuditLog.NotFound",
                "The requested audit record does not exist."));
        }

        return new AuditEntryDetailDto(
            entry.Id,
            entry.UserId,
            entry.OccurredAt,
            entry.SchemaName,
            entry.TableName,
            entry.RecordId,
            entry.OperationType,
            entry.HoldingId,
            entry.OrganizationId,
            ParseChanges(entry.ChangesJson));
    }

    /// <summary>
    /// Parses the interceptor's JSON payload (an array of
    /// { field, oldValue, newValue }) into typed changes. Values that
    /// are not JSON strings are rendered with <see cref="JsonElement.ToString()"/>.
    /// </summary>
    private static IReadOnlyList<AuditFieldChangeDto> ParseChanges(string changesJson)
    {
        if (string.IsNullOrWhiteSpace(changesJson))
        {
            return [];
        }

        using var document = JsonDocument.Parse(changesJson);
        var root = document.RootElement;

        if (root.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var changes = new List<AuditFieldChangeDto>();

        foreach (var element in root.EnumerateArray())
        {
            if (element.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            var field = element.TryGetProperty("field", out var fieldElement)
                ? fieldElement.GetString() ?? string.Empty
                : string.Empty;

            changes.Add(new AuditFieldChangeDto(
                field,
                FormatValue(element, "oldValue"),
                FormatValue(element, "newValue")));
        }

        return changes;
    }

    private static string? FormatValue(JsonElement change, string propertyName)
    {
        if (!change.TryGetProperty(propertyName, out var value) ||
            value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        return value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : value.ToString();
    }
}