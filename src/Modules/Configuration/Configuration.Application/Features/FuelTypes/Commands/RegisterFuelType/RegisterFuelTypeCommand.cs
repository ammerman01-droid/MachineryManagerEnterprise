using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.FuelTypes.Commands.RegisterFuelType;

/// <summary>Command to register a new FuelType within a Holding.</summary>
public sealed record RegisterFuelTypeCommand(Guid HoldingId, string Name, long Price, FuelKind Kind)
    : IRequest<Result<Guid>>;