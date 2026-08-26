using MachineryManager.Asset.Application.Features.EngineModels.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Queries.GetEngineModelById;

/// <summary>Query to retrieve a single Engine Model by its identifier.</summary>
public sealed record GetEngineModelByIdQuery(Guid EngineModelId) : IRequest<Result<EngineModelDto>>;