using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RegisterLubricantType;

/// <summary>Registers a new Lubricant Type option within a Holding.</summary>
public sealed record RegisterLubricantTypeCommand(Guid HoldingId, string Name) : IRequest<Result<Guid>>;
