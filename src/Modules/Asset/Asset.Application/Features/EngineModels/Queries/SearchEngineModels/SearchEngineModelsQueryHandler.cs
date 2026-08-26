using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Queries.SearchEngineModels;

/// <summary>Handles <see cref="SearchEngineModelsQuery"/> by delegating to the repository search projection.</summary>
public sealed class SearchEngineModelsQueryHandler
    : IRequestHandler<SearchEngineModelsQuery, Result<SearchEngineModelsResponse>>
{
    private readonly IEngineModelRepository _engineModelRepository;

    /// <summary>Initializes a new instance of the <see cref="SearchEngineModelsQueryHandler"/> class.</summary>
    public SearchEngineModelsQueryHandler(IEngineModelRepository engineModelRepository)
    {
        _engineModelRepository = engineModelRepository;
    }

    /// <summary>Executes the search query.</summary>
    public async Task<Result<SearchEngineModelsResponse>> Handle(
        SearchEngineModelsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _engineModelRepository.SearchAsync(
            request.HoldingId,
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}