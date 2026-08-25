using System.Text.RegularExpressions;

namespace MachineryManager.Identity.Domain;

/// <summary>
/// The password policy as explicitly specified by the product owner
/// (chat, 2026-08-18) — not an invented or library-default rule.
/// </summary>
public static class PasswordPolicy
{
    /// <summary>Minimum password length.</summary>
    public const int MinLength = 8;

    /// <summary>Matches only English letters, digits, and standard ASCII special/punctuation characters (no other alphabets or Unicode).</summary>
    public static readonly Regex AllowedCharacters = new(
        @"^[A-Za-z0-9!""#$%&'()*+,\-./:;<=>?@\[\]^_`{|}~]*$",
        RegexOptions.Compiled);
}