using MachineryManager.Asset.Application.Features.EngineModels.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Queries.SearchEngineModels;

/// <summary>Query to search Engine Models within an Organization.</summary>
public sealed record SearchEngineModelsQuery(Guid OrganizationId, string? SearchTerm, int Page = 1, int PageSize = 20)
    : IRequest<Result<SearchEngineModelsResponse>>;

/// <summary>Paginated response for <see cref="SearchEngineModelsQuery"/>.</summary>
public sealed record SearchEngineModelsResponse(
    IReadOnlyCollection<EngineModelDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);