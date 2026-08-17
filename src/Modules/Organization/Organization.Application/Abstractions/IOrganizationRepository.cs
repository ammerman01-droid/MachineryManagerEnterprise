using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Abstractions;

/// <summary>
/// Repository contract for the <see cref="global::Organization.Domain.Organization"/> aggregate,
/// extending the generic aggregate root repository with search capabilities.
/// </summary>
public interface IOrganizationRepository
    : IRepository<global::Organization.Domain.Organization, OrganizationId>
{
    /// <summary>
    /// Performs a paginated search over organizations with an optional text filter.
    /// </summary>
    /// <param name="searchTerm">Optional text to filter by organization name.</param>
    /// <param name="page">The one-based page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the matching items and pagination metadata.</returns>
    Task<Features.Organizations.Queries.SearchOrganizations.SearchOrganizationsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}