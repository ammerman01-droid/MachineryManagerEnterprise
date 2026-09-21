using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure.Tests;
using MachineryManagerEnterprise.Identity.Infrastructure.Validation;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests.Validation;

/// <summary>
/// Verifies <see cref="UsernameLengthValidator"/> enforces
/// <see cref="UsernamePolicy.MaxLength"/> (chat, 2026-08-18).
/// </summary>
public sealed class UsernameLengthValidatorTests
{
    private readonly UsernameLengthValidator _sut = new();
    private readonly UserManager<ApplicationUser> _userManager = FakeUserManager.Create();

    [Fact]
    public async Task ValidateAsync_WithUsernameAtMaxLength_Succeeds()
    {
        var user = new ApplicationUser { UserName = new string('a', UsernamePolicy.MaxLength) };

        var result = await _sut.ValidateAsync(_userManager, user);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WithUsernameOneOverMaxLength_FailsWithUserNameTooLong()
    {
        var user = new ApplicationUser { UserName = new string('a', UsernamePolicy.MaxLength + 1) };

        var result = await _sut.ValidateAsync(_userManager, user);

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "UserNameTooLong");
    }

    [Fact]
    public async Task ValidateAsync_WithShortUsername_Succeeds()
    {
        var user = new ApplicationUser { UserName = "sysadmin" };

        var result = await _sut.ValidateAsync(_userManager, user);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WithNullUsername_Succeeds()
    {
        // Matches the implementation's explicit null/empty guard: this
        // validator only judges length, not presence.
        var user = new ApplicationUser { UserName = null };

        var result = await _sut.ValidateAsync(_userManager, user);

        result.Succeeded.Should().BeTrue();
    }
}
