using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;

/// <summary>
/// Handles <see cref="RegisterAssetModelCommand"/> by verifying the
/// Holding and Company, invoking domain registration, persisting the
/// aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterAssetModelCommandHandler
    : IRequestHandler<RegisterAssetModelCommand, Result<Guid>>
{
    private const string RequiredPermission = "Asset.Create";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterAssetModelCommandHandler"/> class.</summary>
    /// <param name="assetModelRepository">The Asset Model repository.</param>
    /// <param name="holdingLookupService">Cross-module lookup for Holding existence.</param>
    /// <param name="configurationLookupService">Cross-module lookup for Configuration-module master data (Company, in this handler).</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public RegisterAssetModelCommandHandler(
        IAssetModelRepository assetModelRepository,
        IHoldingLookupService holdingLookupService,
        IConfigurationLookupService configurationLookupService,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _holdingLookupService = holdingLookupService;
        _configurationLookupService = configurationLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new Asset Model's identifier, or a business error.</returns>
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

        var holdingExists = await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken);

        if (!holdingExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.HoldingNotFound(request.HoldingId));
        }

        // Cross-module validation (chat, 2026-09-01): the Company
        // referenced must actually exist and belong to this same
        // Holding — mirrors the equivalent check already present in
        // RegisterEngineModelCommandHandler. Without this, any GUID
        // (even a non-existent one) was silently accepted.
        var companyExists = await _configurationLookupService.CompanyExistsInHoldingAsync(
            request.CompanyId, request.HoldingId, cancellationToken);

        if (!companyExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetModelErrors.CompanyNotFound(request.CompanyId));
        }

        var result = global::Asset.Domain.AssetModel.Register(
            request.HoldingId,
            request.Name,
            request.CompanyId,
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