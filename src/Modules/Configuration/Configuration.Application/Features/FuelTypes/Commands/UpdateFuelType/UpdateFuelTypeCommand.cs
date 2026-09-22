using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.UpdateFuelType;

/// <summary>Updates an existing Fuel Type's name, price, and kind.</summary>
/// <param name="Id">The identifier of the Fuel Type to update.</param>
/// <param name="Name">The new display name.</param>
/// <param name="Price">The new price.</param>
/// <param name="Kind">The new fixed fuel-kind classification.</param>
public sealed record UpdateFuelTypeCommand(Guid Id, string Name, long Price, FuelKind Kind) : IRequest<Result>;
