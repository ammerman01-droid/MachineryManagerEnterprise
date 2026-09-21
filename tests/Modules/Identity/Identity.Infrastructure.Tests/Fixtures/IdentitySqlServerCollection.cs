using Microsoft.Data.SqlClient;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests;

/// <summary>
/// xunit collection definition tying this module's SQL Server-backed
/// test classes to the ONE shared container fixture from
/// <c>MachineryManagerEnterprise.Testing.Common</c> (TE-0030 / ADR-0024)
/// — Identity does not spin up its own separate SQL Server container.
/// </summary>
[CollectionDefinition(Name)]
public sealed class IdentitySqlServerCollection
    : ICollectionFixture<MachineryManagerEnterprise.Testing.Common.SqlServerContainerFixture>
{
    /// <summary>The collection name test classes pass to <c>[Collection(...)]</c>.</summary>
    public const string Name = "Identity SQL Server collection";
}

/// <summary>
/// The shared fixture exposes one connection string per container, not
/// per test class — this gives each test class its own isolated
/// database inside that same container, so classes never see one
/// another's data despite sharing the container.
/// </summary>
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
