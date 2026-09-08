using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RenameEngineModel;

/// <summary>Command to rename an existing Engine Model.</summary>
public sealed record RenameEngineModelCommand(Guid EngineModelId, string Name) : IRequest<Result>;