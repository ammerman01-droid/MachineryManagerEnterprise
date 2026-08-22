using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Queries.SearchProfiles;

/// <summary>Handles <see cref="SearchProfilesQuery"/> by delegating to the repository.</summary>
public sealed class SearchProfilesQueryHandler
    : IRequestHandler<SearchProfilesQuery, Result<SearchProfilesResponse>>
{
    private readonly IProfileRepository _profileRepository;

    /// <summary>Initializes a new instance of the <see cref="SearchProfilesQueryHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    public SearchProfilesQueryHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    /// <summary>Executes the search query and returns the paginated result.</summary>
    /// <param name="request">The search query parameters.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the paginated search response.</returns>
    public async Task<Result<SearchProfilesResponse>> Handle(
        SearchProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _profileRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}