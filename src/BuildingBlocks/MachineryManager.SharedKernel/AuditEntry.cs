namespace MachineryManager.SharedKernel;

/// <summary>
/// A single row of the platform-wide, append-only database audit
/// trail (chat, 2026-09-05) — records who changed what, when, and how,
/// across every module's schema. Written exclusively by
/// <c>AuditSaveChangesInterceptor</c> (SharedKernel.Infrastructure,
/// gam 2); never created or modified by module business logic.
/// </summary>
/// <remarks>
/// Deliberately NOT an <see cref="AggregateRoot{TId}"/>: it raises no
/// Domain Events and has no business invariants beyond the structural
/// checks in <see cref="Create"/> — it is a pure, immutable log record.
/// </remarks>
public sealed class AuditEntry : Entity<Guid>
{
    /// <summary>Gets the identifier of the user who made the change, or <c>null</c> if not attributable to an authenticated user.</summary>
    public Guid? UserId { get; private set; }

    /// <summary>Gets the UTC timestamp when the change was committed.</summary>
    public DateTimeOffset OccurredAt { get; private set; }

    /// <summary>Gets the database schema of the changed table (e.g. "organization", "asset").</summary>
    public string SchemaName { get; private set; }

    /// <summary>Gets the name of the changed table.</summary>
    public string TableName { get; private set; }

    /// <summary>Gets the primary key of the changed record, as a string (to accommodate varying key types).</summary>
    public string RecordId { get; private set; }

    /// <summary>Gets the kind of change that occurred.</summary>
    public AuditOperationType OperationType { get; private set; }

    /// <summary>
    /// Gets the changed fields as a JSON array of { field, oldValue,
    /// newValue } objects. Sensitive fields are redacted by the
    /// interceptor before this value is ever set (gam 2 — Exclusion List).
    /// </summary>
    public string ChangesJson { get; private set; }

    /// <summary>Gets the owning Holding's identifier, if the changed entity exposes one.</summary>
    public Guid? HoldingId { get; private set; }

    /// <summary>Gets the owning Organization's identifier, if the changed entity exposes one.</summary>
    public Guid? OrganizationId { get; private set; }

    // Reserved for EF Core materialization only.
    private AuditEntry()
    {
        SchemaName = string.Empty;
        TableName = string.Empty;
        RecordId = string.Empty;
        ChangesJson = string.Empty;
    }

    private AuditEntry(
        Guid id,
        Guid? userId,
        DateTimeOffset occurredAt,
        string schemaName,
        string tableName,
        string recordId,
        AuditOperationType operationType,
        string changesJson,
        Guid? holdingId,
        Guid? organizationId)
        : base(id)
    {
        UserId = userId;
        OccurredAt = occurredAt;
        SchemaName = schemaName;
        TableName = tableName;
        RecordId = recordId;
        OperationType = operationType;
        ChangesJson = changesJson;
        HoldingId = holdingId;
        OrganizationId = organizationId;
    }

    /// <summary>
    /// Creates a new Audit Entry. Called exclusively by
    /// <c>AuditSaveChangesInterceptor</c> — never by module business logic.
    /// </summary>
    /// <param name="userId">The identifier of the user who made the change, if known.</param>
    /// <param name="occurredAt">The UTC timestamp of the change.</param>
    /// <param name="schemaName">The database schema of the changed table.</param>
    /// <param name="tableName">The name of the changed table.</param>
    /// <param name="recordId">The primary key of the changed record, as a string.</param>
    /// <param name="operationType">The kind of change.</param>
    /// <param name="changesJson">The changed fields, already redacted where necessary, as a JSON string.</param>
    /// <param name="holdingId">The owning Holding's identifier, if resolvable.</param>
    /// <param name="organizationId">The owning Organization's identifier, if resolvable.</param>
    /// <returns>The new <see cref="AuditEntry"/>.</returns>
    public static AuditEntry Create(
        Guid? userId,
        DateTimeOffset occurredAt,
        string schemaName,
        string tableName,
        string recordId,
        AuditOperationType operationType,
        string changesJson,
        Guid? holdingId,
        Guid? organizationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(schemaName);
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordId);

        return new AuditEntry(
            Guid.NewGuid(),
            userId,
            occurredAt,
            schemaName,
            tableName,
            recordId,
            operationType,
            changesJson,
            holdingId,
            organizationId);
    }
}