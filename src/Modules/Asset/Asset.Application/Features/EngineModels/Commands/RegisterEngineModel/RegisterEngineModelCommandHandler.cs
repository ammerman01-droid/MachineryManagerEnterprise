using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RegisterEngineModel;

/// <summary>
/// Handles <see cref="RegisterEngineModelCommand"/> by invoking domain
/// registration, persisting the aggregate, and committing the unit of
/// work.
/// </summary>
public sealed class RegisterEngineModelCommandHandler
    : IRequestHandler<RegisterEngineModelCommand, Result<Guid>>
{
    private const string RequiredPermission = "Asset.Create";

    private readonly IEngineModelRepository _engineModelRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterEngineModelCommandHandler"/> class.</summary>
    public RegisterEngineModelCommandHandler(
        IEngineModelRepository engineModelRepository,
        IHoldingLookupService holdingLookupService,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _engineModelRepository = engineModelRepository;
        _holdingLookupService = holdingLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterEngineModelCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(request.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        // Validate the Holding actually exists before creating
        // Holding-scoped catalog data (chat, 2026-08-26 — gap fix).
        var holdingExists = await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken);

        if (!holdingExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.EngineModelErrors.HoldingNotFound(request.HoldingId));
        }

        var result = global::Asset.Domain.EngineModel.Register(
            request.HoldingId,
            request.Name,
            request.Manufacturer,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _engineModelRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
