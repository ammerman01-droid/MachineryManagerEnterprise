using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Holdings.Commands.RegisterHolding;

/// <summary>Command to register a new Holding.</summary>
public sealed record RegisterHoldingCommand(string Name)
    : IRequest<Result<Guid>>;