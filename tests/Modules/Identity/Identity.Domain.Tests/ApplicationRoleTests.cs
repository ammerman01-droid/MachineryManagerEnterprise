using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Domain.Tests;

/// <summary>Verifies <see cref="ApplicationRole"/>'s two constructors.</summary>
public sealed class ApplicationRoleTests
{
    [Fact]
    public void IsAnIdentityRoleOfGuid()
    {
        var role = new ApplicationRole();

        role.Should().BeAssignableTo<IdentityRole<Guid>>();
    }

    [Fact]
    public void ParameterlessConstructor_LeavesNameUnset()
    {
        var role = new ApplicationRole();

        role.Name.Should().BeNull();
    }

    [Fact]
    public void NameConstructor_SetsName()
    {
        var role = new ApplicationRole(StandardRoles.FleetManager);

        role.Name.Should().Be(StandardRoles.FleetManager);
    }

    [Fact]
    public void NameConstructor_DoesNotGenerateAnId()
    {
        // IdentityRole<Guid> (the generic base ApplicationRole extends)
        // does NOT auto-generate an Id in its constructor — only the
        // non-generic IdentityRole : IdentityRole<string> special-cases
        // that. So the Id stays Guid.Empty here; it's EF Core's
        // ValueGeneratedOnAdd() convention for Guid keys that assigns a
        // real value, at SaveChanges time (see IdentityDbContextTests /
        // IdentityDataSeederTests for that behavior against a real
        // database). This test exists specifically to correct the
        // opposite (wrong) assumption from an earlier draft.
        var role = new ApplicationRole(StandardRoles.Operator);

        role.Id.Should().Be(Guid.Empty);
    }
}
