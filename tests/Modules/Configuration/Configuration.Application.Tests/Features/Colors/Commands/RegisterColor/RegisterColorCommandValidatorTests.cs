using FluentAssertions;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.RegisterColor;
using Xunit;

namespace MachineryManagerEnterprise.Configuration.Application.Tests.Features.Colors.Commands.RegisterColor;

/// <summary>Tests for <see cref="RegisterColorCommandValidator"/>.</summary>
public sealed class RegisterColorCommandValidatorTests
{
    private readonly RegisterColorCommandValidator _validator = new();

    [Fact]
    public void Validate_WithAValidCommand_HasNoErrors()
    {
        var command = new RegisterColorCommand(Guid.NewGuid(), "Red");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithAnEmptyHoldingId_HasAnErrorOnHoldingId()
    {
        var command = new RegisterColorCommand(Guid.Empty, "Red");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterColorCommand.HoldingId));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithAMissingName_HasAnErrorOnName(string? name)
    {
        var command = new RegisterColorCommand(Guid.NewGuid(), name!);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterColorCommand.Name));
    }

    [Fact]
    public void Validate_WithANameLongerThanTheMaximum_HasAnErrorOnName()
    {
        var tooLongName = new string('a', global::Configuration.Domain.Color.MaxNameLength + 1);
        var command = new RegisterColorCommand(Guid.NewGuid(), tooLongName);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterColorCommand.Name));
    }
}
