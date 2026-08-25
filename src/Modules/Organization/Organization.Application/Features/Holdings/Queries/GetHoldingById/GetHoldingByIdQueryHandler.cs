using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Holdings.Dtos;
using MachineryManager.SharedKernel;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Holdings.Queries.GetHoldingById;

/// <summary>
/// Handles <see cref="GetHoldingByIdQuery"/> by loading the aggregate
/// and projecting it into a read-only DTO.
/// </summary>
public sealed class GetHoldingByIdQueryHandler
    : IRequestHandler<GetHoldingByIdQuery, Result<HoldingDto>>
{
    private readonly IHoldingRepository _holdingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetHoldingByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="holdingRepository">The holding repository.</param>
    public GetHoldingByIdQueryHandler(IHoldingRepository holdingRepository)
    {
        _holdingRepository = holdingRepository;
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

        var dto = new HoldingDto(holding.Id.Value, holding.Name);
        return Result.Success(dto);
    }
}