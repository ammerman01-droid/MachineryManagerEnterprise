using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.SearchProfiles;

/// <summary>
/// Handles <see cref="SearchProfilesQuery"/>, returning only the profiles the caller may see:
/// everything for Platform users, otherwise only the caller's own Holding's profiles.
/// </summary>
public sealed class SearchProfilesQueryHandler
    : IRequestHandler<SearchProfilesQuery, Result<SearchProfilesResponse>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IProfileAuthorizationService _authorization;

    /// <summary>Initializes a new instance of the <see cref="SearchProfilesQueryHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="authorization">Decides which profiles the caller may see.</param>
    public SearchProfilesQueryHandler(
        IProfileRepository profileRepository,
        IProfileAuthorizationService authorization)
    {
        _profileRepository = profileRepository;
        _authorization = authorization;
    }

    /// <inheritdoc />
    public async Task<Result<SearchProfilesResponse>> Handle(
        SearchProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var visibility = await _authorization.ResolveVisibilityAsync(cancellationToken);

        if (visibility.IsFailure)
        {
            return Result.Failure<SearchProfilesResponse>(visibility.Error);
        }

        var response = await _profileRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            visibility.Value,
            cancellationToken);

        return Result.Success(response);
    }
}
