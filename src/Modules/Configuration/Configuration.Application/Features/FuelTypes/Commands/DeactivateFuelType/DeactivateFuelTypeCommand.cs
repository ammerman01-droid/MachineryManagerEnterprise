using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.DeactivateFuelType;

/// <summary>Deactivates (soft-deletes) an existing Fuel Type.</summary>
/// <param name="Id">The identifier of the Fuel Type to deactivate.</param>
public sealed record DeactivateFuelTypeCommand(Guid Id) : IRequest<Result>;
