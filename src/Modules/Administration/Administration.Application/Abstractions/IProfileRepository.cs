using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.SearchProfiles;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Administration.Domain.Profile"/> aggregate.</summary>
public interface IProfileRepository : IRepository<global::Administration.Domain.Profile, global::Administration.Domain.ProfileId>
{
    /// <summary>Performs a paginated search over profiles.</summary>
    Task<SearchProfilesResponse> SearchAsync(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}