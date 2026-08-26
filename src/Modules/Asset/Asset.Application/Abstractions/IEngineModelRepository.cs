using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Asset.Domain.EngineModel"/> aggregate.</summary>
public interface IEngineModelRepository : IRepository<global::Asset.Domain.EngineModel, global::Asset.Domain.EngineModelId>
{
    /// <summary>
    /// Performs a paginated search over engine models within the given
    /// Organization.
    /// </summary>
    Task<Features.EngineModels.Queries.SearchEngineModels.SearchEngineModelsResponse> SearchAsync(
        Guid organizationId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}