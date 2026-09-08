namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup for Holding existence, needed by
/// other modules (e.g. Asset) to validate a HoldingId they receive
/// before creating Holding-scoped catalog data (chat, 2026-08-26).
/// Defined here so a module other than Organization can depend on the
/// contract without depending on Organization.Domain/Infrastructure
/// directly, mirroring <see cref="IOrganizationLookupService"/> and
/// <see cref="IPermissionEvaluator"/>.
/// </summary>
public interface IHoldingLookupService
{
    /// <summary>Determines whether a Holding with the given identifier currently exists.</summary>
    /// <param name="holdingId">The Holding's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><see langword="true"/> if the Holding exists; otherwise <see langword="false"/>.</returns>
    Task<bool> ExistsAsync(Guid holdingId, CancellationToken cancellationToken = default);
}
