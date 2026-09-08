using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Organization.Application.Features.Holdings.Commands.RegisterHolding;

/// <summary>Command to register a new Holding.</summary>
public sealed record RegisterHoldingCommand(string Name)
    : IRequest<Result<Guid>>;