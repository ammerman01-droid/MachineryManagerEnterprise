namespace MachineryManager.SharedKernel;

/// <summary>The kind of database change a single <see cref="AuditEntry"/> represents.</summary>
public enum AuditOperationType
{
    /// <summary>A new record was inserted.</summary>
    Created = 0,

    /// <summary>An existing record was modified.</summary>
    Updated = 1,

    /// <summary>An existing record was deleted.</summary>
    Deleted = 2,
}