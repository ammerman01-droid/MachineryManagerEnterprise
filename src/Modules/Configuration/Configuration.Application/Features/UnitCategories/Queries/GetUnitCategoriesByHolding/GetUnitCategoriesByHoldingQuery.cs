using MachineryManager.Configuration.Application.Features.UnitCategories.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.UnitCategories.Queries.GetUnitCategoriesByHolding;

/// <summary>Retrieves the list of Unit Categories defined for a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding.</param>
public sealed record GetUnitCategoriesByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<UnitCategoryDto>>>;