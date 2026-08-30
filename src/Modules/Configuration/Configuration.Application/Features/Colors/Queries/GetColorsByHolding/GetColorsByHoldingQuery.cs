using MachineryManager.Configuration.Application.Features.Colors.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.Colors.Queries.GetColorsByHolding;

/// <summary>Retrieves the list of Color options defined for a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding.</param>
public sealed record GetColorsByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<ColorDto>>>;