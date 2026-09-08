using MachineryManagerEnterprise.Organization.Application.Features.Holdings.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Organization.Application.Features.Holdings.Queries.GetHoldingById;

/// <summary>
/// Query to retrieve a single holding by its unique identifier.
/// </summary>
/// <param name="HoldingId">The GUID of the holding to retrieve.</param>
public sealed record GetHoldingByIdQuery(Guid HoldingId)
    : IRequest<Result<HoldingDto>>;