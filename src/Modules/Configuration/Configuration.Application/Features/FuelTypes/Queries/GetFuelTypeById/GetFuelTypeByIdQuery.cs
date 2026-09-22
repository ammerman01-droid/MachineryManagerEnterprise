using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypeById;

/// <summary>Retrieves a single Fuel Type by its identifier (used to pre-fill the Edit form).</summary>
/// <param name="Id">The identifier of the Fuel Type to retrieve.</param>
public sealed record GetFuelTypeByIdQuery(Guid Id) : IRequest<Result<FuelTypeDto>>;
