using MachineryManager.Organization.Application.Features.Holdings.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Holdings.Queries.GetHoldingById;

/// <summary>
/// Query to retrieve a single holding by its unique identifier.
/// </summary>
/// <param name="HoldingId">The GUID of the holding to retrieve.</param>
public sealed record GetHoldingByIdQuery(Guid HoldingId)
    : IRequest<Result<HoldingDto>>;