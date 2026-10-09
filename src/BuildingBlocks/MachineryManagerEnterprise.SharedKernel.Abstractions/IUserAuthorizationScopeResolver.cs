namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Resolves the current tenant-scope of a user from their ACTIVE
/// profile assignment, so cross-cutting consumers — e.g. the Identity
/// claims pipeline — can embed scope identifiers in authentication
/// artifacts without depending on the Administration or Organization
/// modules directly (mirrors IPermissionEvaluator decoupling, ADR-0030).
/// </summary>
public interface IUserAuthorizationScopeResolver
{
    /// <summary>
    /// Resolves the user's current scope identifiers, backfilling the
    /// parent chain (Project → Organization → Holding) via the
    /// Organization module's read-only lookups.
    /// </summary>
    /// <returns>
    /// The resolved scope — members are null when the user is not
    /// scoped to that level; <see langword="null"/> when the user has
    /// no active assignment at all.
    /// </returns>
    Task<ResolvedUserScope?> ResolveAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// The user's current scope identifiers. A null member means "not
/// scoped to that level" — for a Platform-level user ALL members are
/// null, which is the correct representation of unrestricted access.
/// </summary>
public sealed record ResolvedUserScope(Guid? HoldingId, Guid? OrganizationId, Guid? ProjectId);