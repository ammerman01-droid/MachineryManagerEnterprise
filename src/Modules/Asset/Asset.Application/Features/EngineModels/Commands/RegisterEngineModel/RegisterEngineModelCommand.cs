using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RegisterEngineModel;

/// <summary>Command to register a new Engine Model within a Holding.</summary>
public sealed record RegisterEngineModelCommand(Guid HoldingId, string Name, string Manufacturer)
    : IRequest<Result<Guid>>;