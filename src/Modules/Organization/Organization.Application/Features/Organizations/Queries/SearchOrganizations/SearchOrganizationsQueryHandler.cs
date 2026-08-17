using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Organizations.Queries.SearchOrganizations;

/// <summary>
/// Handles <see cref="SearchOrganizationsQuery"/> by delegating to the repository
/// search projection.
/// </summary>
public sealed class SearchOrganizationsQueryHandler
    : IRequestHandler<SearchOrganizationsQuery, Result<SearchOrganizationsResponse>>
{
    private readonly IOrganizationRepository _organizationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchOrganizationsQueryHandler"/> class.
    /// </summary>
    /// <param name="organizationRepository">The organization repository.</param>
    public SearchOrganizationsQueryHandler(
        IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    /// <summary>
    /// Executes the search query and returns the paginated result.
    /// </summary>
    /// <param name="request">The search query parameters.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the paginated search response.</returns>
    public async Task<Result<SearchOrganizationsResponse>> Handle(
        SearchOrganizationsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _organizationRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}