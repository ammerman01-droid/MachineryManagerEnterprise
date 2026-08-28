using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Holdings.Commands.RegisterHolding;

/// <summary>
/// Handles <see cref="RegisterHoldingCommand"/> by orchestrating domain
/// registration, persisting the aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterHoldingCommandHandler
    : IRequestHandler<RegisterHoldingCommand, Result<Guid>>
{
    private const string RequiredPermission = "Holding.Manage";

    private readonly IHoldingRepository _holdingRepository;
    private readonly IOrganizationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterHoldingCommandHandler"/> class.
    /// </summary>
    /// <param name="holdingRepository">The holding repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public RegisterHoldingCommandHandler(
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
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(HoldingErrors.NotAuthorized());
        }

        // Registering a new Holding is a platform-level action (it has
        // no HoldingId of its own yet) — checked against
        // ResourceScope.PlatformWide, mirroring RegisterOrganization.
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            ResourceScope.PlatformWide,
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(HoldingErrors.NotAuthorized());
        }

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