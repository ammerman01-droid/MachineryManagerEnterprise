namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Describes the full scope chain of a specific resource being
/// accessed (e.g. a Project belongs to an Organization, which may
/// belong to a Holding), so <see cref="IPermissionEvaluator"/> can
/// check it against a user's assignments without needing to know
/// about other modules' data. The CALLING module is responsible for
/// resolving this full chain from its own local data before calling
/// the evaluator (chat, 2026-08-22).
/// </summary>
/// <param name="HoldingId">The Holding this resource belongs to, if any.</param>
/// <param name="OrganizationId">The Organization this resource belongs to, if any.</param>
/// <param name="ProjectId">The Project this resource belongs to, if any.</param>
public sealed record ResourceScope(Guid? HoldingId, Guid? OrganizationId, Guid? ProjectId)
{
    /// <summary>A scope representing the platform itself (used for platform-wide actions, e.g. registering a new standalone Organization).</summary>
    public static ResourceScope PlatformWide { get; } = new(null, null, null);
}