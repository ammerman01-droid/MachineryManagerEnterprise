namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup for Organization tenant-hierarchy
/// facts needed by other modules to resolve <see cref="ResourceScope"/>
/// (e.g. Asset resources belong to an Organization, which itself may
/// belong to a Holding — chat, 2026-08-25). Defined here so a module
/// other than Organization can depend on the contract without
/// depending on Organization.Domain/Infrastructure directly, mirroring
/// how <see cref="IPermissionEvaluator"/> decouples Administration.
/// </summary>
public interface IOrganizationLookupService
{
    /// <summary>
    /// Resolves the Holding that the given Organization currently
    /// belongs to, if any.
    /// </summary>
    /// <param name="organizationId">The organization's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Holding's identifier, or <see langword="null"/> if the organization is unassigned or does not exist.</returns>
    Task<Guid?> GetHoldingIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
}