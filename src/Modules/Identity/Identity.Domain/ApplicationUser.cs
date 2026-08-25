using Microsoft.AspNetCore.Identity;

namespace MachineryManager.Identity.Domain;

/// <summary>
/// The platform's authenticated principal. Per ADR-0030, Identity is a
/// Platform Module (not a DDD Bounded Context), so this type extends
/// ASP.NET Core Identity's <see cref="IdentityUser{TKey}"/> directly
/// rather than <c>AggregateRoot&lt;TId&gt;</c> as business modules
/// (e.g. Organization) do.
/// </summary>
/// <remarks>
/// Per ADR-0030, Identity NEVER owns Organization data: this type
/// intentionally carries no Organization reference. Associating a
/// User with an Organization is a business operation owned by the
/// Organization module (AssociateUserWithOrganization command,
/// 05-application Section 5.3), not by Identity.
/// </remarks>
public sealed class ApplicationUser : IdentityUser<Guid>
{
}