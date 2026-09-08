using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.EngineModels.Commands.RenameEngineModel;

/// <summary>Command to rename an existing Engine Model.</summary>
public sealed record RenameEngineModelCommand(Guid EngineModelId, string Name) : IRequest<Result>;