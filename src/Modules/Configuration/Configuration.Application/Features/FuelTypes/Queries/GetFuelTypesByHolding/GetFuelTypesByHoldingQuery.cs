using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypesByHolding;

/// <summary>Query to retrieve every Fuel Type registered for a Holding.</summary>
public sealed record GetFuelTypesByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<FuelTypeDto>>>;