using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypesByHolding;

/// <summary>Query to retrieve every Fuel Type registered for a Holding.</summary>
/// <param name="HoldingId">The Holding whose fuel type catalog should be returned.</param>
/// <param name="IncludeInactive">
/// When <see langword="true"/>, deactivated (soft-deleted) fuel types
/// are included in the result (used by the admin management page).
/// Defaults to <see langword="false"/>.
/// </param>
public sealed record GetFuelTypesByHoldingQuery(Guid HoldingId, bool IncludeInactive = false)
    : IRequest<Result<IReadOnlyList<FuelTypeDto>>>;
