using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace MachineryManager.SharedKernel.Infrastructure;

/// <summary>
/// Automatically records an <see cref="AuditEntry"/> for every tracked
/// Added/Modified/Deleted entity, as part of the same SaveChanges
/// transaction as the business change itself
/// (chat, 2026-09-05 — Audit Log module, gam 2).
/// </summary>
/// <remarks>
/// <para>
/// Registered as Scoped (it depends on the Scoped
/// <see cref="ICurrentUserService"/>) and must be supplied to each
/// module's <c>AddDbContext</c> call via the (IServiceProvider,
/// DbContextOptionsBuilder) overload — see gam 3. Attaching this
/// interceptor to a DbContext WITHOUT also mapping
/// <see cref="AuditEntry"/> in that DbContext's model (via
/// <see cref="AuditEntryMapping"/>) will throw at runtime, since
/// <c>context.Set&lt;AuditEntry&gt;()</c> requires the entity to be part
/// of the model.
/// </para>
/// <para>
/// Security invariant (product owner, chat 2026-09-05): the Identity
/// schema is NEVER audited — not even the bare fact that a change
/// occurred. Entities mapped to the "identity" schema are skipped
/// entirely, so no row mentioning Identity can ever exist in
/// audit.AuditEntry. The Identity module's DbContext must therefore
/// never have this interceptor attached and never map AuditEntry.
/// </para>
/// </remarks>
public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Schemas that are never audited at all — not even a bare
    /// "a change happened" row. Currently just "identity", whose tables
    /// hold credentials, tokens, and security stamps (product owner,
    /// chat 2026-09-05: no trace of the Identity schema may ever appear
    /// in the audit log).
    /// </summary>
    private static readonly HashSet<string> NeverAuditedSchemas = new(StringComparer.OrdinalIgnoreCase)
    {
        "identity",
    };

    /// <summary>
    /// Property name fragments that are always masked, regardless of
    /// schema — a defense-in-depth net for sensitive fields added to
    /// any module in the future.
    /// </summary>
    /// <remarks>
    /// Deliberately uses the precise fragment "securitystamp" rather
    /// than "stamp", which would also match ordinary fields like
    /// "Timestamp" and over-redact them.
    /// </remarks>
    private static readonly string[] SensitivePropertyNameFragments =
    [
        "password", "secret", "token", "securitystamp", "payload",
    ];

    private const string RedactedPlaceholder = "[REDACTED]";

    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>Initializes a new instance of the <see cref="AuditSaveChangesInterceptor"/> class.</summary>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    public AuditSaveChangesInterceptor(ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is { } context)
        {
            foreach (var auditEntry in BuildAuditEntries(context))
            {
                context.Set<AuditEntry>().Add(auditEntry);
            }
        }

        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is { } context)
        {
            foreach (var auditEntry in BuildAuditEntries(context))
            {
                context.Set<AuditEntry>().Add(auditEntry);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private List<AuditEntry> BuildAuditEntries(DbContext context)
    {
        var entries = new List<AuditEntry>();
        var occurredAt = _dateTimeProvider.UtcNow;
        var userId = _currentUserService.UserId;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            // Never audit the audit table itself.
            if (entry.Entity is AuditEntry)
            {
                continue;
            }

            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
            {
                continue;
            }

            var schemaName = entry.Metadata.GetSchema() ?? "dbo";

            // Security invariant (product owner, chat 2026-09-05): the
            // Identity schema leaves NO trace in the audit log — the
            // entry is skipped entirely, not merely redacted.
            if (NeverAuditedSchemas.Contains(schemaName))
            {
                continue;
            }

            var tableName = entry.Metadata.GetTableName() ?? entry.Metadata.ClrType.Name;

            var operationType = entry.State switch
            {
                EntityState.Added => AuditOperationType.Created,
                EntityState.Deleted => AuditOperationType.Deleted,
                _ => AuditOperationType.Updated,
            };

            // Updates with no actually-changed scalar properties (e.g. a
            // no-op SaveChanges on an untouched tracked entity) produce
            // no meaningful audit signal.
            if (operationType == AuditOperationType.Updated && !entry.Properties.Any(p => p.IsModified))
            {
                continue;
            }

            var recordId = GetRecordId(entry);
            var changesJson = BuildChangesJson(entry, operationType);

            entries.Add(AuditEntry.Create(
                userId,
                occurredAt,
                schemaName,
                tableName,
                recordId,
                operationType,
                changesJson,
                TryGetGuidProperty(entry, "HoldingId"),
                TryGetGuidProperty(entry, "OrganizationId")));
        }

        return entries;
    }

    private static string GetRecordId(EntityEntry entry)
    {
        var keyValues = entry.Properties
            .Where(p => p.Metadata.IsPrimaryKey())
            .Select(p => (p.CurrentValue ?? p.OriginalValue)?.ToString() ?? "?");

        return string.Join(",", keyValues);
    }

    private static Guid? TryGetGuidProperty(EntityEntry entry, string propertyName)
    {
        var property = entry.Metadata.FindProperty(propertyName);

        if (property is null)
        {
            return null;
        }

        var value = entry.CurrentValues[property] ?? entry.OriginalValues[property];

        return value as Guid?;
    }

    private static bool IsSensitivePropertyName(string propertyName) =>
        SensitivePropertyNameFragments.Any(fragment =>
            propertyName.Contains(fragment, StringComparison.OrdinalIgnoreCase));

    private static string BuildChangesJson(EntityEntry entry, AuditOperationType operationType)
    {
        var changes = new List<object>();

        foreach (var property in entry.Properties)
        {
            if (operationType == AuditOperationType.Updated && !property.IsModified)
            {
                continue;
            }

            object? oldValue = operationType == AuditOperationType.Created ? null : property.OriginalValue;
            object? newValue = operationType == AuditOperationType.Deleted ? null : property.CurrentValue;

            if (IsSensitivePropertyName(property.Metadata.Name))
            {
                oldValue = oldValue is null ? null : RedactedPlaceholder;
                newValue = newValue is null ? null : RedactedPlaceholder;
            }

            changes.Add(new { field = property.Metadata.Name, oldValue, newValue });
        }

        return JsonSerializer.Serialize(changes);
    }
}