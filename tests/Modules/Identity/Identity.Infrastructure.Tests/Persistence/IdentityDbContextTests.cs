using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Tests;
using MachineryManagerEnterprise.Identity.Infrastructure.Persistence;
using MachineryManagerEnterprise.Testing.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests.Persistence;

/// <summary>
/// Integration tests for <see cref="IdentityDbContext"/> against a real
/// SQL Server (the shared Testing.Common container, TE-0030 / ADR-0024)
/// — the "identity" default schema, the OpenIddict EF Core model, and
/// the ThemeMode / ThemeCornerStyle column constraints are all
/// SQL-Server-specific and not meaningfully verifiable against a
/// relational-agnostic provider.
/// </summary>
[Collection(IdentitySqlServerCollection.Name)]
public sealed class IdentityDbContextTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _containerFixture;
    private string _connectionString = string.Empty;

    public IdentityDbContextTests(SqlServerContainerFixture containerFixture)
    {
        _containerFixture = containerFixture;
    }

    public async Task InitializeAsync()
    {
        _connectionString = TestDatabaseNaming.UniqueConnectionString(
            _containerFixture.ConnectionString, nameof(IdentityDbContextTests));

        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
    }

    private IdentityDbContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

        optionsBuilder.UseSqlServer(
            _connectionString,
            sqlServerOptions => sqlServerOptions.MigrationsHistoryTable("__EFMigrationsHistory", schema: "identity"));

        optionsBuilder.UseOpenIddict<Guid>();

        return new IdentityDbContext(optionsBuilder.Options);
    }

    [Fact]
    public async Task Model_PlacesIdentityTablesUnderTheIdentitySchema()
    {
        await using var context = CreateContext();

        var schema = await context.Database.SqlQuery<string>(
                $"SELECT TABLE_SCHEMA AS [Value] FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUsers'")
            .SingleAsync();

        schema.Should().Be("identity");
    }

    [Fact]
    public async Task Model_CreatesTheOpenIddictApplicationsTableUnderTheIdentitySchema()
    {
        await using var context = CreateContext();

        var exists = await context.Database.SqlQuery<int>(
                $"""
                 SELECT COUNT(*) AS [Value] FROM INFORMATION_SCHEMA.TABLES
                 WHERE TABLE_SCHEMA = 'identity' AND TABLE_NAME = 'OpenIddictApplications'
                 """)
            .SingleAsync();

        exists.Should().Be(1, "IdentityDbContext hosts the OpenIddict EF Core stores per ADR-0030");
    }

    [Fact]
    public async Task Users_RoundTripsThemePreferenceFields()
    {
        var userId = Guid.NewGuid();

        await using (var writeContext = CreateContext())
        {
            writeContext.Users.Add(new ApplicationUser
            {
                Id = userId,
                UserName = "roundtrip-user",
                NormalizedUserName = "ROUNDTRIP-USER",
                ThemeMode = "Dark",
                ThemeCornerStyle = "Rounded",
            });

            await writeContext.SaveChangesAsync();
        }

        await using var readContext = CreateContext();
        var reloaded = await readContext.Users.SingleAsync(u => u.Id == userId);

        reloaded.ThemeMode.Should().Be("Dark");
        reloaded.ThemeCornerStyle.Should().Be("Rounded");
    }

    [Fact]
    public async Task Users_ThemeModeLongerThan20Characters_FailsToSave()
    {
        await using var context = CreateContext();

        context.Users.Add(new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "overflow-user",
            NormalizedUserName = "OVERFLOW-USER",
            ThemeMode = new string('x', 21),
        });

        var act = () => context.SaveChangesAsync();

        // HasMaxLength(20) maps to nvarchar(20); SQL Server rejects the
        // oversized value at write time rather than truncating it.
        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task Roles_RoundTripsName()
    {
        var roleId = Guid.NewGuid();

        await using (var writeContext = CreateContext())
        {
            writeContext.Roles.Add(new ApplicationRole("Fleet Manager") { Id = roleId, NormalizedName = "FLEET MANAGER" });
            await writeContext.SaveChangesAsync();
        }

        await using var readContext = CreateContext();
        var reloaded = await readContext.Roles.SingleAsync(r => r.Id == roleId);

        reloaded.Name.Should().Be("Fleet Manager");
    }
}
