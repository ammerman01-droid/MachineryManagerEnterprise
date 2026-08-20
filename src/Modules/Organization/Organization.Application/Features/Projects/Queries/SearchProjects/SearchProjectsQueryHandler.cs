using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Projects.Queries.SearchProjects;

/// <summary>
/// Handles <see cref="SearchProjectsQuery"/> by delegating to the repository
/// search projection.
/// </summary>
public sealed class SearchProjectsQueryHandler
    : IRequestHandler<SearchProjectsQuery, Result<SearchProjectsResponse>>
{
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchProjectsQueryHandler"/> class.
    /// </summary>
    /// <param name="projectRepository">The project repository.</param>
    public SearchProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Executes the search query and returns the paginated result.
    /// </summary>
    /// <param name="request">The search query parameters.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the paginated search response.</returns>
    public async Task<Result<SearchProjectsResponse>> Handle(
        SearchProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _projectRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}