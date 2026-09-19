using Configuration.Domain;
using FluentAssertions;
using MachineryManagerEnterprise.Configuration.Application.Mappings;
using MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Testing.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Tests.Persistence;

/// <summary>
/// Groups every Configuration-module integration test onto one shared
/// <see cref="SqlServerContainerFixture"/> so the container starts once
/// per test run instead of once per test class.
/// </summary>
[CollectionDefinition(nameof(ConfigurationDatabaseCollection))]
public sealed class ConfigurationDatabaseCollection : ICollectionFixture<SqlServerContainerFixture>;

/// <summary>
/// Integration tests for <see cref="ColorRepository"/> against a real SQL
/// Server (Testcontainers), per TE-0030/ADR-0024. Each test gets its own
/// <see cref="ConfigurationDbContext"/> instance; rows are left in place
/// after each test (the container itself is disposed at the end of the
/// run), so every test uses fresh random Guids rather than relying on
/// row-level cleanup.
/// </summary>
[Collection(nameof(ConfigurationDatabaseCollection))]
public sealed class ColorRepositoryTests : IAsyncLifetime
{
    private readonly SqlServerContainerFixture _dbFixture;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private ConfigurationDbContext _dbContext = null!;

    public ColorRepositoryTests(SqlServerContainerFixture dbFixture)
    {
        _dbFixture = dbFixture;
        _dateTimeProvider.UtcNow.Returns(DateTimeOffset.UtcNow);
    }

    public async Task InitializeAsync()
    {
        _dbContext = CreateContext();

        // No EF Core migrations exist for this module yet. Once they do
        // (ADR-0037), replace this with `await _dbContext.Database.MigrateAsync();`
        // so these tests exercise the same schema-creation path as production
        // and catch drift between migrations and the current model.
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync() => await _dbContext.DisposeAsync();

    private ConfigurationDbContext CreateContext() => new(
        new DbContextOptionsBuilder<ConfigurationDbContext>()
            .UseSqlServer(_dbFixture.ConnectionString)
            .Options);

    [Fact]
    public async Task Add_ThenSaveChanges_PersistsTheColorSoItCanBeReadBackFromAFreshContext()
    {
        var holdingId = Guid.NewGuid();
        var color = Color.Register(holdingId, "Red", _dateTimeProvider).Value;
        var sut = new ColorRepository(_dbContext, Substitute.For<IMapper>());

        sut.Add(color);
        await _dbContext.SaveChangesAsync();

        // Read through a brand-new context/repository so the assertion
        // proves the row round-tripped through SQL Server rather than
        // just remaining in the first context's change tracker.
        await using var readContext = CreateContext();
        var readSut = new ColorRepository(readContext, Substitute.For<IMapper>());

        var found = await readSut.GetByIdAsync(color.Id);

        found.Should().NotBeNull();
        found!.Name.Should().Be("Red");
        found.HoldingId.Should().Be(holdingId);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNoColorMatchesTheId_ReturnsNull()
    {
        var sut = new ColorRepository(_dbContext, Substitute.For<IMapper>());

        var found = await sut.GetByIdAsync(ColorId.New());

        found.Should().BeNull();
    }

    [Fact]
    public async Task Remove_ThenSaveChanges_DeletesTheColorFromTheDatabase()
    {
        var color = Color.Register(Guid.NewGuid(), "Green", _dateTimeProvider).Value;
        var sut = new ColorRepository(_dbContext, Substitute.For<IMapper>());
        sut.Add(color);
        await _dbContext.SaveChangesAsync();

        sut.Remove(color);
        await _dbContext.SaveChangesAsync();

        var found = await sut.GetByIdAsync(color.Id);
        found.Should().BeNull();
    }

    [Fact]
    public async Task GetByHoldingAsync_ReturnsOnlyThatHoldingsColorsOrderedByName()
    {
        var holdingId = Guid.NewGuid();
        var otherHoldingId = Guid.NewGuid();
        var red = Color.Register(holdingId, "Red", _dateTimeProvider).Value;
        var blue = Color.Register(holdingId, "Blue", _dateTimeProvider).Value;
        var otherHoldingColor = Color.Register(otherHoldingId, "Yellow", _dateTimeProvider).Value;

        // Uses the module's real Mapster registration (ConfigurationMappingConfig)
        // rather than a substitute, so this test also catches drift between the
        // entity, the DTO and the mapping rule itself (e.g. the ColorId -> Guid
        // conversion), not just the repository's query/ordering logic.
        var mapperConfig = new TypeAdapterConfig();
        mapperConfig.Scan(typeof(ConfigurationMappingConfig).Assembly);
        var mapper = new Mapper(mapperConfig);

        var sut = new ColorRepository(_dbContext, mapper);
        sut.Add(red);
        sut.Add(blue);
        sut.Add(otherHoldingColor);
        await _dbContext.SaveChangesAsync();

        var result = await sut.GetByHoldingAsync(holdingId);

        result.Select(c => c.Name).Should().ContainInOrder("Blue", "Red");
        result.Should().NotContain(c => c.Name == "Yellow");
    }
}
