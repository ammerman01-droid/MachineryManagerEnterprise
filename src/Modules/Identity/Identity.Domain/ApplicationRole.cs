using Microsoft.AspNetCore.Identity;

namespace MachineryManager.Identity.Domain;

/// <summary>
/// A platform Role, per the Authorization Model (05-application,
/// Section 5.8): User → Role → Permission → Business Operation.
/// </summary>
public sealed class ApplicationRole : IdentityRole<Guid>
{
    /// <summary>Initializes a new instance of the <see cref="ApplicationRole"/> class.</summary>
    public ApplicationRole()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ApplicationRole"/> class with the given name.</summary>
    /// <param name="roleName">The role's display name.</param>
    public ApplicationRole(string roleName)
        : base(roleName)
    {
    }
}