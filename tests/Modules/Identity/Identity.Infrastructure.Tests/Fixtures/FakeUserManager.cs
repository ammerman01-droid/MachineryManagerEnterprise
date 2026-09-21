using MachineryManagerEnterprise.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests;

/// <summary>
/// Constructs a <see cref="UserManager{TUser}"/> whose members can be
/// stubbed with NSubstitute. <see cref="UserManager{TUser}"/>'s public
/// members are virtual specifically to support this pattern — there is
/// no first-class fake/mock shipped by ASP.NET Core Identity itself.
/// </summary>
internal static class FakeUserManager
{
    /// <summary>
    /// Creates a substitute <see cref="UserManager{TUser}"/> backed by a
    /// substitute <see cref="IUserStore{TUser}"/>. All other constructor
    /// dependencies are passed as <see langword="null"/>: safe here
    /// because production code being tested only ever calls the
    /// UserManager's own public (virtual) methods — which are configured
    /// per-test via <c>manager.SomeMethod(...).Returns(...)</c> — never
    /// the internals that would dereference those dependencies directly.
    /// </summary>
    public static UserManager<ApplicationUser> Create(IUserStore<ApplicationUser>? store = null)
    {
        store ??= Substitute.For<IUserStore<ApplicationUser>>();

        return Substitute.For<UserManager<ApplicationUser>>(
            store,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
    }
}
