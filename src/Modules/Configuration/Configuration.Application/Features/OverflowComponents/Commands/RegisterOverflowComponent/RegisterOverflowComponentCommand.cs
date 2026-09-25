using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.RegisterOverflowComponent;

/// <summary>Registers a new Overflow Component option within a Holding.</summary>
public sealed record RegisterOverflowComponentCommand(Guid HoldingId, string Name) : IRequest<Result<Guid>>;
