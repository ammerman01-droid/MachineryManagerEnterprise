using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RegisterEngineModel;

/// <summary>Command to register a new Engine Model within an Organization.</summary>
public sealed record RegisterEngineModelCommand(Guid OrganizationId, string Name, string Manufacturer)
    : IRequest<Result<Guid>>;