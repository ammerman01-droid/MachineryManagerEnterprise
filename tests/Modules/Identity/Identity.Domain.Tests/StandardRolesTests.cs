using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Domain.Tests;

/// <summary>
/// Verifies <see cref="StandardRoles"/> against the closed catalog
/// documented in 05-application, Section 5.8 (Authorization Model).
/// Per the AI Engineering Contract's Business Rules governance, this
/// catalog is closed: these tests exist specifically to catch an
/// accidental (undocumented) addition or removal of a role.
/// </summary>
public sealed class StandardRolesTests
{
    private static readonly string[] ExpectedRoles =
    [
        "System Administrator",
        "Organization Administrator",
        "Fleet Manager",
        "Maintenance Manager",
        "Maintenance Technician",
        "Workshop Supervisor",
        "Operator",
        "Financial Officer",
        "Procurement Officer",
        "Document Controller",
        "Read-Only Auditor",
    ];

    [Fact]
    public void All_ContainsExactlyTheDocumentedElevenRoles()
    {
        StandardRoles.All.Should().BeEquivalentTo(ExpectedRoles);
    }

    [Fact]
    public void All_ContainsNoDuplicates()
    {
        StandardRoles.All.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void All_HasElevenEntries()
    {
        StandardRoles.All.Should().HaveCount(11);
    }

    [Theory]
    [InlineData(nameof(StandardRoles.SystemAdministrator), StandardRoles.SystemAdministrator, "System Administrator")]
    [InlineData(nameof(StandardRoles.OrganizationAdministrator), StandardRoles.OrganizationAdministrator, "Organization Administrator")]
    [InlineData(nameof(StandardRoles.FleetManager), StandardRoles.FleetManager, "Fleet Manager")]
    [InlineData(nameof(StandardRoles.MaintenanceManager), StandardRoles.MaintenanceManager, "Maintenance Manager")]
    [InlineData(nameof(StandardRoles.MaintenanceTechnician), StandardRoles.MaintenanceTechnician, "Maintenance Technician")]
    [InlineData(nameof(StandardRoles.WorkshopSupervisor), StandardRoles.WorkshopSupervisor, "Workshop Supervisor")]
    [InlineData(nameof(StandardRoles.Operator), StandardRoles.Operator, "Operator")]
    [InlineData(nameof(StandardRoles.FinancialOfficer), StandardRoles.FinancialOfficer, "Financial Officer")]
    [InlineData(nameof(StandardRoles.ProcurementOfficer), StandardRoles.ProcurementOfficer, "Procurement Officer")]
    [InlineData(nameof(StandardRoles.DocumentController), StandardRoles.DocumentController, "Document Controller")]
    [InlineData(nameof(StandardRoles.ReadOnlyAuditor), StandardRoles.ReadOnlyAuditor, "Read-Only Auditor")]
    public void EachConstant_HasTheDocumentedDisplayName(string constantName, string actualValue, string expectedValue)
    {
        actualValue.Should().Be(expectedValue, because: $"{constantName} must match the documented display name exactly");
    }

    [Fact]
    public void All_IsInTheSameOrderAsTheIndividualConstants()
    {
        // Guards against someone reordering `All` without noticing —
        // seeding order itself is not meaningful, but an unexplained
        // reorder is a signal something else changed too.
        StandardRoles.All.Should().Equal(ExpectedRoles);
    }
}
