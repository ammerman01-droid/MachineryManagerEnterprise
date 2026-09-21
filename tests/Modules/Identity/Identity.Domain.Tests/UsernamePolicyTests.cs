using FluentAssertions;
using MachineryManagerEnterprise.Identity.Domain;
using Xunit;

namespace MachineryManagerEnterprise.Identity.Domain.Tests;

/// <summary>
/// Verifies <see cref="UsernamePolicy"/> matches the value explicitly
/// specified by the product owner (chat, 2026-08-18).
/// </summary>
public sealed class UsernamePolicyTests
{
    [Fact]
    public void MaxLength_Is20()
    {
        UsernamePolicy.MaxLength.Should().Be(20);
    }
}
