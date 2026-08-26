using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.EngineModels.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Queries.GetEngineModelById;

/// <summary>Handles <see cref="GetEngineModelByIdQuery"/> by loading the aggregate and mapping it to a DTO.</summary>
public sealed class GetEngineModelByIdQueryHandler
    : IRequestHandler<GetEngineModelByIdQuery, Result<EngineModelDto>>
{
    private readonly IEngineModelRepository _engineModelRepository;

    /// <summary>Initializes a new instance of the <see cref="GetEngineModelByIdQueryHandler"/> class.</summary>
    public GetEngineModelByIdQueryHandler(IEngineModelRepository engineModelRepository)
    {
        _engineModelRepository = engineModelRepository;
    }

    /// <summary>Executes the lookup use case.</summary>
    public async Task<Result<EngineModelDto>> Handle(GetEngineModelByIdQuery request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.EngineModelId.From(request.EngineModelId);
        var engineModel = await _engineModelRepository.GetByIdAsync(id, cancellationToken);

        if (engineModel is null)
        {
            return Result.Failure<EngineModelDto>(
                Error.NotFound("EngineModel.NotFound", $"Engine model with id {request.EngineModelId} was not found."));
        }

        var dto = new EngineModelDto(engineModel.Id.Value, engineModel.Name, engineModel.Manufacturer);

        return Result.Success(dto);
    }
}