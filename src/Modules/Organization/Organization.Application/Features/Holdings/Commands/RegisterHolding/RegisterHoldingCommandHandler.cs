using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Holdings.Commands.RegisterHolding;

/// <summary>
/// Handles <see cref="RegisterHoldingCommand"/> by orchestrating domain
/// registration, persisting the aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterHoldingCommandHandler
    : IRequestHandler<RegisterHoldingCommand, Result<Guid>>
{
    private readonly IHoldingRepository _holdingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterHoldingCommandHandler"/> class.
    /// </summary>
    /// <param name="holdingRepository">The holding repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public RegisterHoldingCommandHandler(
        IHoldingRepository holdingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _holdingRepository = holdingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Executes the registration use case.
    /// </summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new holding's GUID on success.</returns>
    public async Task<Result<Guid>> Handle(
        RegisterHoldingCommand request,
        CancellationToken cancellationToken)
    {
        var result = global::Organization.Domain.Holding.Register(request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _holdingRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}