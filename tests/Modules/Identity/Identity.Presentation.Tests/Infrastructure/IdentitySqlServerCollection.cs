using Microsoft.Data.SqlClient;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Presentation.Tests.Infrastructure;

/// <summary>
/// xunit collection definition tying this module's SQL Server-backed
/// endpoint tests to the ONE shared container fixture from
/// <c>MachineryManagerEnterprise.Testing.Common</c> (TE-0030 / ADR-0024).
/// A separate collection from <c>Identity.Infrastructure.Tests</c>'s
/// own (they run in different test assemblies, so this does not start a
/// second container — each assembly gets exactly one, per its own
/// collection).
/// </summary>
[CollectionDefinition(Name)]
public sealed class IdentitySqlServerCollection
    : ICollectionFixture<MachineryManagerEnterprise.Testing.Common.SqlServerContainerFixture>
{
    /// <summary>The collection name test classes pass to <c>[Collection(...)]</c>.</summary>
    public const string Name = "Identity Presentation SQL Server collection";
}

/// <summary>Gives each test class its own isolated database inside the one shared container.</summary>
internal static class TestDatabaseNaming
{
    /// <summary>Builds a connection string pointing at a fresh, uniquely-named database inside the running container.</summary>
    /// <param name="baseConnectionString">The shared fixture's connection string.</param>
    /// <param name="databaseNamePrefix">A short, human-readable prefix (e.g. the test class name) to ease debugging failed runs.</param>
    public static string UniqueConnectionString(string baseConnectionString, string databaseNamePrefix)
    {
        var builder = new SqlConnectionStringBuilder(baseConnectionString)
        {
            InitialCatalog = $"{databaseNamePrefix}_{Guid.NewGuid():N}",
        };

        return builder.ConnectionString;
    }
}
