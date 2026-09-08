using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Queries.GetColorsByHolding;

/// <summary>Retrieves the list of Color options defined for a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding.</param>
public sealed record GetColorsByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<ColorDto>>>;