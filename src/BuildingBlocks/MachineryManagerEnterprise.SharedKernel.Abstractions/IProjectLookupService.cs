namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup for Project tenant-hierarchy facts,
/// mirroring <see cref="IOrganizationLookupService"/> and
/// <see cref="IHoldingLookupService"/>.
/// </summary>
public interface IProjectLookupService
{
    /// <summary>
    /// Determines whether a Project with the given identifier currently exists.
    /// </summary>
    Task<bool> ExistsAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves the Organization that the given Project belongs to, if any.
    /// </summary>
    Task<Guid?> GetOrganizationIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves the Holding that owns the given Project's Organization, if any.
    /// </summary>
    Task<Guid?> GetHoldingIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);
    
        /// <summary>
    /// Gets a Project's current name — needed to auto-name a Work
    /// Calendar at lazy-creation time (chat, 2026-09-16).
    /// </summary>
    /// <param name="projectId">The Project's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Project's name, or <see langword="null"/> if the Project does not exist.</returns>
    Task<string?> GetNameAsync(Guid projectId, CancellationToken cancellationToken = default);
}