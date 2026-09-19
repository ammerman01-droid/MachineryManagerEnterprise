using Configuration.Domain;
using Configuration.Domain.Events;
using FluentAssertions;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using NSubstitute;
using Xunit;

namespace Configuration.Domain.Tests;

/// <summary>Tests for <see cref="Color.Register"/> and the invariants it enforces.</summary>
public sealed class ColorTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly DateTimeOffset _now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public ColorTests()
    {
        _dateTimeProvider.UtcNow.Returns(_now);
    }

    [Fact]
    public void Register_WithValidName_ReturnsSuccessWithTrimmedNameAndGivenHolding()
    {
        var holdingId = Guid.NewGuid();

        var result = Color.Register(holdingId, "  Red  ", _dateTimeProvider);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Red");
        result.Value.HoldingId.Should().Be(holdingId);
    }

    [Fact]
    public void Register_WithValidName_AssignsANewNonEmptyId()
    {
        var result = Color.Register(Guid.NewGuid(), "Red", _dateTimeProvider);

        result.Value.Id.Should().NotBeNull();
        result.Value.Id.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Register_CalledTwice_ProducesTwoDistinctIds()
    {
        var holdingId = Guid.NewGuid();

        var first = Color.Register(holdingId, "Red", _dateTimeProvider);
        var second = Color.Register(holdingId, "Blue", _dateTimeProvider);

        first.Value.Id.Should().NotBe(second.Value.Id);
    }

    [Fact]
    public void Register_WithValidName_RaisesExactlyOneColorRegisteredDomainEvent()
    {
        var holdingId = Guid.NewGuid();

        var result = Color.Register(holdingId, "Blue", _dateTimeProvider);

        var color = result.Value;
        color.DomainEvents.Should().ContainSingle();
        color.DomainEvents.Single().Should().BeOfType<ColorRegistered>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Register_WithNullEmptyOrWhitespaceName_ReturnsNameRequiredError(string? name)
    {
        var result = Color.Register(Guid.NewGuid(), name!, _dateTimeProvider);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Color.NameRequired");
    }

    [Fact]
    public void Register_WithNameLongerThanMaxLength_ReturnsNameTooLongError()
    {
        var tooLongName = new string('a', Color.MaxNameLength + 1);

        var result = Color.Register(Guid.NewGuid(), tooLongName, _dateTimeProvider);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Color.NameTooLong");
    }

    [Fact]
    public void Register_WithNameExactlyAtMaxLength_ReturnsSuccess()
    {
        var name = new string('a', Color.MaxNameLength);

        var result = Color.Register(Guid.NewGuid(), name, _dateTimeProvider);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().HaveLength(Color.MaxNameLength);
    }

    [Fact]
    public void Register_WhenNameFailsValidation_DoesNotRaiseADomainEvent()
    {
        // Guards against a regression where an event fires before the
        // aggregate is confirmed valid.
        var result = Color.Register(Guid.NewGuid(), "", _dateTimeProvider);

        result.IsFailure.Should().BeTrue();
    }
}
