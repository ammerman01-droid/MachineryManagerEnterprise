using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.GetFuelConsumptionById;

/// <summary>
/// Query to retrieve a single fuel-consumption record by its
/// identifier (chat, 2026-09-22 — needed to pre-fill the Edit form,
/// mirroring GetAssetById/GetPersonnelById).
/// </summary>
public sealed record GetFuelConsumptionByIdQuery(Guid Id) : IRequest<Result<FuelConsumptionDto>>;
