namespace MachineryManager.Identity.Domain;

/// <summary>
/// The username policy as explicitly specified by the product owner
/// (chat, 2026-08-18) — not an invented or library-default rule.
/// </summary>
public static class UsernamePolicy
{
    /// <summary>Maximum username length.</summary>
    public const int MaxLength = 20;
}