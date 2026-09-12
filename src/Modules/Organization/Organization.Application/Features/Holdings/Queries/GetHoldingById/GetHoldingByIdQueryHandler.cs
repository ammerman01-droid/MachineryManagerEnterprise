using MachineryManagerEnterprise.Organization.Application.Abstractions;
using MachineryManagerEnterprise.Organization.Application.Features.Holdings.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MapsterMapper;
using MediatR;
using Organization.Domain;

namespace MachineryManagerEnterprise.Organization.Application.Features.Holdings.Queries.GetHoldingById;

/// <summary>
/// Handles <see cref="GetHoldingByIdQuery"/> by loading the aggregate
/// and projecting it into a read-only DTO.
/// </summary>
public sealed class GetHoldingByIdQueryHandler
    : IRequestHandler<GetHoldingByIdQuery, Result<HoldingDto>>
{
    private readonly IHoldingRepository _holdingRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetHoldingByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="holdingRepository">The holding repository.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetHoldingByIdQueryHandler(IHoldingRepository holdingRepository, IMapper mapper)
    {
        _holdingRepository = holdingRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Executes the query and returns the holding DTO if found.
    /// </summary>
    /// <param name="request">The query containing the holding identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="HoldingDto"/> or a not-found error.</returns>
    public async Task<Result<HoldingDto>> Handle(
        GetHoldingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var holdingId = HoldingId.From(request.HoldingId);
        var holding = await _holdingRepository.GetByIdAsync(holdingId, cancellationToken);

        if (holding is null)
        {
            return Result.Failure<HoldingDto>(
                Error.NotFound(
                    "Holding.NotFound",
                    $"Holding with id {request.HoldingId} was not found."));
        }

        return Result.Success(_mapper.Map<HoldingDto>(holding));
    }
}