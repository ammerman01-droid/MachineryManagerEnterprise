using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Tests;
using MachineryManagerEnterprise.Identity.Infrastructure.Validation;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests.Validation;

/// <summary>
/// Verifies <see cref="AllowedCharactersPasswordValidator"/> enforces
/// <see cref="PasswordPolicy.AllowedCharacters"/> (chat, 2026-08-18).
/// </summary>
public sealed class AllowedCharactersPasswordValidatorTests
{
    private readonly AllowedCharactersPasswordValidator _sut = new();
    private readonly UserManager<ApplicationUser> _userManager = FakeUserManager.Create();

    [Theory]
    [InlineData("Passw0rd!")]
    [InlineData("abcXYZ123-_.")]
    public async Task ValidateAsync_WithAllowedCharactersOnly_Succeeds(string password)
    {
        var result = await _sut.ValidateAsync(_userManager, new ApplicationUser(), password);

        result.Succeeded.Should().BeTrue();
    }

    [Theory]
    [InlineData("Pässwörd1")]
    [InlineData("پسورد12345")]
    [InlineData("emoji😀word1")]
    public async Task ValidateAsync_WithDisallowedCharacters_FailsWithPasswordInvalidCharacters(string password)
    {
        var result = await _sut.ValidateAsync(_userManager, new ApplicationUser(), password);

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "PasswordInvalidCharacters");
    }

    [Fact]
    public async Task ValidateAsync_WithNullPassword_Succeeds()
    {
        // Matches the implementation's explicit null-check: this validator
        // only judges character content, so a null password (rejected
        // elsewhere, e.g. by the required-length check) is not its concern.
        var result = await _sut.ValidateAsync(_userManager, new ApplicationUser(), password: null);

        result.Succeeded.Should().BeTrue();
    }
}
