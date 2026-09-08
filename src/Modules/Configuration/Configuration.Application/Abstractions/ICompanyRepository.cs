using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Configuration.Application.Abstractions;

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
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Companies belonging to the Holding, ordered by name.</returns>
    Task<IReadOnlyList<Features.Companies.Dtos.CompanyDto>> GetByHoldingAsync(
        Guid holdingId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether a Company with the specified name already exists in the Holding.
    /// </summary>
    Task<bool> ExistsByNameInHoldingAsync(
        Guid holdingId,
        string name,
        CancellationToken cancellationToken = default);
}