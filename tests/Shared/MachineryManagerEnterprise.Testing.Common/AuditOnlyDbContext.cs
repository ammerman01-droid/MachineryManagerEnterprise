using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Testing.Common;

/// <summary>
/// A throwaway <see cref="DbContext"/> whose only job is to create the
/// shared <c>audit.AuditEntry</c> table in an isolated module-slice test
/// database.
/// </summary>
/// <remarks>
/// In production, that table is created by exactly one migration — the
/// Administration module's DbContext, which calls
/// <c>ApplyAuditEntryMapping(ownsTable: true)</c>. Every other module
/// passes <c>ownsTable: false</c>, which excludes the table from ITS
/// migrations. A module-slice test host that only boots one module
/// (e.g. Configuration) never gets that table for free — but the
/// module's <c>AuditSaveChangesInterceptor</c> still tries to write to
/// it on every <c>SaveChanges</c>, since the interceptor is attached
/// regardless.
///
/// <para>
/// Usage in a module's own <c>&lt;Module&gt;ApiFactory</c>: call
/// <c>EnsureCreatedAsync()</c> on an instance of THIS context first
/// (before anything else touches the same connection string), then
/// create the module's own tables on its real DbContext via
/// <c>context.GetService&lt;IRelationalDatabaseCreator&gt;().CreateTablesAsync()</c>
/// — NOT <c>EnsureCreatedAsync()</c> again. EF Core's
/// <c>EnsureCreatedAsync</c> only creates a database's tables the first
/// time the physical database itself is created; once it already
/// exists, a second context calling <c>EnsureCreatedAsync</c> sees the
/// database is there and silently creates none of its own tables.
/// <c>CreateTablesAsync</c> is the documented workaround for exactly
/// this multiple-DbContexts-one-database scenario.
/// </para>
/// </remarks>
public sealed class AuditOnlyDbContext(DbContextOptions<AuditOnlyDbContext> options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ownsTable: true here is specific to this throwaway context in
        // this test database — it does NOT make the real module's
        // production DbContext own the table; that stays ownsTable: false
        // there, exactly as in production.
        modelBuilder.ApplyAuditEntryMapping(ownsTable: true);
    }
}
