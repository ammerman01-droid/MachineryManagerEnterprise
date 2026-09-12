using Microsoft.AspNetCore.Identity;

namespace MachineryManagerEnterprise.Identity.Domain;

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
    /// <summary>
    /// The user's saved UI theme colour scheme — the string name of an
    /// <c>AppThemeMode</c> value (e.g. <c>"Light"</c>, <c>"Dark"</c>,
    /// <c>"Colorful"</c>), or <see langword="null"/> if the user has never
    /// saved a preference.
    /// </summary>
    public string? ThemeMode { get; set; }

    /// <summary>
    /// The user's saved UI corner treatment — the string name of an
    /// <c>AppCornerStyle</c> value (e.g. <c>"Sharp"</c>, <c>"Rounded"</c>),
    /// or <see langword="null"/> if the user has never saved a preference.
    /// </summary>
    public string? ThemeCornerStyle { get; set; }
}