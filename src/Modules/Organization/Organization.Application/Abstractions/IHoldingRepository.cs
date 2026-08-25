using MachineryManager.SharedKernel.Abstractions;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Abstractions;

/// <summary>Repository contract for the <see cref="Holding"/> aggregate.</summary>
public interface IHoldingRepository : IRepository<Holding, HoldingId>
{
    /// <summary>
    /// Performs a paginated search over holdings, restricted to the given
    /// authorized scope (Phase 3 — Scope-based Filtering).
    /// </summary>
    Task<Features.Holdings.Queries.SearchHoldings.SearchHoldingsResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        AuthorizedScopeSet authorizedScope,
        CancellationToken cancellationToken = default);
}