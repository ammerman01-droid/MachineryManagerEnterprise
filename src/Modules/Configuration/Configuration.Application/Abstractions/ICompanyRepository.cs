using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Configuration.Application.Abstractions;

/// <summary>
/// Repository contract for the Company aggregate.
/// </summary>
public interface ICompanyRepository
    : IRepository<global::Configuration.Domain.Company, global::Configuration.Domain.CompanyId>
{
    /// <summary>
    /// Retrieves all Companies registered for the specified Holding.
    /// </summary>
    /// <param name="holdingId">The Holding whose Company catalog should be returned.</param>
    /// <param name="includeInactive">
    /// When <see langword="true"/>, deactivated (soft-deleted) companies
    /// are included in the result; otherwise only active companies are
    /// returned. Defaults to <see langword="false"/> so existing
    /// consumers keep seeing only active companies without any change.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Companies belonging to the Holding, ordered by name.</returns>
    Task<IReadOnlyList<Features.Companies.Dtos.CompanyDto>> GetByHoldingAsync(
        Guid holdingId,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether a Company with the specified name already exists in the Holding.
    /// </summary>
    Task<bool> ExistsByNameInHoldingAsync(
        Guid holdingId,
        string name,
        CancellationToken cancellationToken = default);
}
