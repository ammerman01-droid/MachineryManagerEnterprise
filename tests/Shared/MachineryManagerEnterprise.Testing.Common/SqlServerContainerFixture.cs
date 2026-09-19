using Testcontainers.MsSql;
using Xunit;

namespace MachineryManagerEnterprise.Testing.Common;

/// <summary>
/// Spins up a single real SQL Server container (via Testcontainers) for
/// the lifetime of an xunit test collection, per TE-0030 (Testing
/// Strategy / ADR-0024) — integration tests run against the same
/// database engine used in production rather than a fake/in-memory
/// provider, and never share EF Core's InMemory provider (it does not
/// enforce the same constraints, e.g. required columns, indexes, or
/// value-conversions like <c>ColorId</c>'s).
///
/// One container is shared per xunit collection (see
/// <see cref="Xunit.CollectionDefinitionAttribute"/> usages in each
/// module's integration-test project) so the ~2-3s SQL Server startup
/// cost is paid once per test run, not once per test class.
/// </summary>
public sealed class SqlServerContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    /// <summary>The connection string to the running container, valid after <see cref="InitializeAsync"/> completes.</summary>
    public string ConnectionString => _container.GetConnectionString();

    /// <inheritdoc />
    public Task InitializeAsync() => _container.StartAsync();

    /// <inheritdoc />
    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}
