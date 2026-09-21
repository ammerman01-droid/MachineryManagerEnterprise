using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Domain.Tests;

/// <summary>
/// Verifies <see cref="ApplicationUser"/>'s own additions on top of
/// <see cref="IdentityUser{TKey}"/> (theme preference fields). ASP.NET
/// Core Identity's own base-class behavior is not re-tested here.
/// </summary>
public sealed class ApplicationUserTests
{
    [Fact]
    public void IsAnIdentityUserOfGuid()
    {
        var user = new ApplicationUser();

        user.Should().BeAssignableTo<IdentityUser<Guid>>();
    }

    [Fact]
    public void ThemeMode_DefaultsToNull()
    {
        var user = new ApplicationUser();

        user.ThemeMode.Should().BeNull();
    }

    [Fact]
    public void ThemeCornerStyle_DefaultsToNull()
    {
        var user = new ApplicationUser();

        user.ThemeCornerStyle.Should().BeNull();
    }

    [Fact]
    public void ThemeMode_CanBeSetAndRead()
    {
        var user = new ApplicationUser { ThemeMode = "Dark" };

        user.ThemeMode.Should().Be("Dark");
    }

    [Fact]
    public void ThemeCornerStyle_CanBeSetAndRead()
    {
        var user = new ApplicationUser { ThemeCornerStyle = "Rounded" };

        user.ThemeCornerStyle.Should().Be("Rounded");
    }

    [Fact]
    public void NeverExposesAnOrganizationReference()
    {
        // Per ADR-0030: Identity NEVER owns Organization data. This test
        // is a structural guard against accidentally reintroducing a
        // "OrganizationId"-shaped property on the platform User — such
        // an association belongs to the Organization module instead.
        var organizationLikeProperties = typeof(ApplicationUser)
            .GetProperties()
            .Where(p => p.Name.Contains("Organization", StringComparison.OrdinalIgnoreCase));

        organizationLikeProperties.Should().BeEmpty(
            "ApplicationUser must not carry an Organization reference (ADR-0030)");
    }
}
