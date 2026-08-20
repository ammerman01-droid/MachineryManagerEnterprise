using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Holdings.Queries.SearchHoldings;

/// <summary>
/// Handles <see cref="SearchHoldingsQuery"/> by delegating to the repository
/// search projection.
/// </summary>
public sealed class SearchHoldingsQueryHandler
    : IRequestHandler<SearchHoldingsQuery, Result<SearchHoldingsResponse>>
{
    private readonly IHoldingRepository _holdingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchHoldingsQueryHandler"/> class.
    /// </summary>
    /// <param name="holdingRepository">The holding repository.</param>
    public SearchHoldingsQueryHandler(IHoldingRepository holdingRepository)
    {
        _holdingRepository = holdingRepository;
    }

    /// <summary>
    /// Executes the search query and returns the paginated result.
    /// </summary>
    /// <param name="request">The search query parameters.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the paginated search response.</returns>
    public async Task<Result<SearchHoldingsResponse>> Handle(
        SearchHoldingsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _holdingRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}