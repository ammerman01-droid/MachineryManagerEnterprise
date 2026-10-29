using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.SearchProfiles;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>Persistence port for the Profile aggregate.</summary>
public interface IProfileRepository : IRepository<global::Administration.Domain.Profile, global::Administration.Domain.ProfileId>
{
    /// <summary>
    /// Searches profiles by name, restricted to what the caller may see.
    /// </summary>
    /// <param name="searchTerm">Optional name filter.</param>
    /// <param name="page">One-based page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="visibility">Which profiles the caller may see; the repository never returns others.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>One page of profiles.</returns>
    Task<SearchProfilesResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        ProfileVisibility visibility,
        CancellationToken cancellationToken = default);

    /// <summary>Finds the system-managed full-access profile of a Holding, if it already exists.</summary>
    /// <param name="holdingId">The Holding.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The system profile, or <see langword="null"/>.</returns>
    Task<global::Administration.Domain.Profile?> GetSystemProfileByHoldingIdAsync(
        Guid holdingId,
        CancellationToken cancellationToken = default);
}
