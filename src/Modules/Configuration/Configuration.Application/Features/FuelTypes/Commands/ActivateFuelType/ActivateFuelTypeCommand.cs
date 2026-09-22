using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.ActivateFuelType;

/// <summary>Reactivates a previously deactivated Fuel Type.</summary>
/// <param name="Id">The identifier of the Fuel Type to reactivate.</param>
public sealed record ActivateFuelTypeCommand(Guid Id) : IRequest<Result>;
