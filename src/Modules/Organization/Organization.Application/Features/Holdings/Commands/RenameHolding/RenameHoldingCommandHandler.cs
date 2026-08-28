using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Holdings.Commands.RenameHolding;

/// <summary>
/// Handles <see cref="RenameHoldingCommand"/> by loading the holding
/// aggregate, invoking the domain rename behavior, and committing the
/// unit of work.
/// </summary>
public sealed class RenameHoldingCommandHandler
    : IRequestHandler<RenameHoldingCommand, Result>
{
    private const string RequiredPermission = "Holding.Edit";

    private readonly IHoldingRepository _holdingRepository;
    private readonly IOrganizationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RenameHoldingCommandHandler"/> class.</summary>
    public RenameHoldingCommandHandler(
        IHoldingRepository holdingRepository,
        IOrganizationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _holdingRepository = holdingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the rename use case.</summary>
    public async Task<Result> Handle(RenameHoldingCommand request, CancellationToken cancellationToken)
    {
        var holdingId = HoldingId.From(request.HoldingId);
        var holding = await _holdingRepository.GetByIdAsync(holdingId, cancellationToken);

        if (holding is null)
        {
            return Result.Failure(
                Error.NotFound("Holding.NotFound", $"Holding with id {request.HoldingId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(HoldingErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holding.Id.Value, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(HoldingErrors.NotAuthorized());
        }

        var result = holding.Rename(request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _holdingRepository.Update(holding);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}