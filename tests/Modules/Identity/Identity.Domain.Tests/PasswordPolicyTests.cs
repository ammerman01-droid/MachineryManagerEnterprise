using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Domain.Tests;

/// <summary>
/// Verifies <see cref="PasswordPolicy"/> matches the values explicitly
/// specified by the product owner (chat, 2026-08-18) — see the policy's
/// own XML doc. These are regression tests against accidental drift,
/// not tests of ASP.NET Core Identity itself.
/// </summary>
public sealed class PasswordPolicyTests
{
    [Fact]
    public void MinLength_Is8()
    {
        PasswordPolicy.MinLength.Should().Be(8);
    }

    [Theory]
    [InlineData("Passw0rd!")]
    [InlineData("abcXYZ123")]
    [InlineData("!\"#$%&'()*+,-./:;<=>?@[]^_`{|}~")]
    [InlineData("")]
    public void AllowedCharacters_MatchesAsciiLettersDigitsAndPunctuation(string password)
    {
        PasswordPolicy.AllowedCharacters.IsMatch(password).Should().BeTrue();
    }

    [Theory]
    [InlineData("Pässwörd1")]     // non-ASCII letters
    [InlineData("پسورد123")]      // Persian characters
    [InlineData("Password\u200C1")] // ZWNJ (common in Persian input) is not an allowed character
    [InlineData("emoji😀123")]
    public void AllowedCharacters_RejectsNonAsciiCharacters(string password)
    {
        PasswordPolicy.AllowedCharacters.IsMatch(password).Should().BeFalse();
    }
}
