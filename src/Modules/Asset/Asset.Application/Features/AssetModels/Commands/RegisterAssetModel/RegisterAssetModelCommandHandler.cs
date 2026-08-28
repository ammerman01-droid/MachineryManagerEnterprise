using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;

/// <summary>
/// Handles <see cref="RegisterAssetModelCommand"/> by invoking domain
/// registration, persisting the aggregate, and committing the unit of
/// work.
/// </summary>
public sealed class RegisterAssetModelCommandHandler
    : IRequestHandler<RegisterAssetModelCommand, Result<Guid>>
{
    private const string RequiredPermission = "Asset.Create";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterAssetModelCommandHandler"/> class.</summary>
    public RegisterAssetModelCommandHandler(
        IAssetModelRepository assetModelRepository,
        IHoldingLookupService holdingLookupService,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _holdingLookupService = holdingLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterAssetModelCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(request.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        // Validate the Holding actually exists before creating
        // Holding-scoped catalog data (chat, 2026-08-26 — gap fix).
        var holdingExists = await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken);

        if (!holdingExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.HoldingNotFound(request.HoldingId));
        }

        var result = global::Asset.Domain.AssetModel.Register(
            request.HoldingId,
            request.Name,
            request.Manufacturer,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _assetModelRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}