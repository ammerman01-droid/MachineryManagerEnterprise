using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using MachineryManagerEnterprise.Identity.Infrastructure;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests;

/// <summary>
/// Verifies <see cref="UserThemePreferenceRepository"/> against a
/// substituted <see cref="UserManager{TUser}"/> — no database involved,
/// since this type is a thin adapter over UserManager.
/// </summary>
public sealed class UserThemePreferenceRepositoryTests
{
    private readonly UserManager<ApplicationUser> _userManager = FakeUserManager.Create();
    private readonly IUserThemePreferenceRepository _sut;

    public UserThemePreferenceRepositoryTests()
    {
        _sut = new UserThemePreferenceRepository(_userManager);
    }

    [Fact]
    public async Task GetAsync_WhenUserNotFound_ReturnsNull()
    {
        var userId = Guid.NewGuid();
        _userManager.FindByIdAsync(userId.ToString()).Returns((ApplicationUser?)null);

        var result = await _sut.GetAsync(userId);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null, "Rounded")]
    [InlineData("Dark", null)]
    [InlineData(null, null)]
    public async Task GetAsync_WhenEitherPreferenceFieldIsUnset_ReturnsNull(string? themeMode, string? cornerStyle)
    {
        var userId = Guid.NewGuid();
        var user = new ApplicationUser { Id = userId, ThemeMode = themeMode, ThemeCornerStyle = cornerStyle };
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);

        var result = await _sut.GetAsync(userId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_WhenBothPreferenceFieldsAreSet_ReturnsThemePreference()
    {
        var userId = Guid.NewGuid();
        var user = new ApplicationUser { Id = userId, ThemeMode = "Dark", ThemeCornerStyle = "Sharp" };
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);

        var result = await _sut.GetAsync(userId);

        result.Should().NotBeNull();
        result!.Value.Mode.Should().Be("Dark");
        result.Value.CornerStyle.Should().Be("Sharp");
    }

    [Fact]
    public async Task SetAsync_WhenUserNotFound_DoesNotCallUpdate()
    {
        var userId = Guid.NewGuid();
        _userManager.FindByIdAsync(userId.ToString()).Returns((ApplicationUser?)null);

        await _sut.SetAsync(userId, new ThemePreference("Dark", "Rounded"));

        await _userManager.DidNotReceive().UpdateAsync(Arg.Any<ApplicationUser>());
    }

    [Fact]
    public async Task SetAsync_WhenUserFound_UpdatesBothFieldsAndCallsUpdate()
    {
        var userId = Guid.NewGuid();
        var user = new ApplicationUser { Id = userId };
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);
        _userManager.UpdateAsync(Arg.Any<ApplicationUser>()).Returns(IdentityResult.Success);

        await _sut.SetAsync(userId, new ThemePreference("Colorful", "Sharp"));

        user.ThemeMode.Should().Be("Colorful");
        user.ThemeCornerStyle.Should().Be("Sharp");
        await _userManager.Received(1).UpdateAsync(user);
    }
}
