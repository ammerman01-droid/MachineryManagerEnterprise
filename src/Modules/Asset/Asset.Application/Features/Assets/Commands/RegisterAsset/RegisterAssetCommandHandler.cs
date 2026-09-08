using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.RegisterAsset;

/// <summary>
/// Handles <see cref="RegisterAssetCommand"/> by validating the
/// referenced Organization, Asset Model, and Color exist and are
/// consistent, ensuring the identification code is unique within the
/// Organization, invoking domain registration, persisting the
/// aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterAssetCommandHandler
    : IRequestHandler<RegisterAssetCommand, Result<Guid>>
{
    private const string RequiredPermission = "Asset.Create";

    private readonly IAssetRepository _assetRepository;
    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterAssetCommandHandler"/> class.</summary>
    /// <param name="assetRepository">The Asset repository.</param>
    /// <param name="assetModelRepository">The Asset Model repository, used to verify the referenced model exists and belongs to the correct Holding.</param>
    /// <param name="configurationLookupService">Cross-module, read-only lookup into the Configuration module, used to verify the referenced Color exists within the correct Holding.</param>
    /// <param name="unitOfWork">The Asset module's Unit of Work, used to commit the new aggregate.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to verify the target Organization exists and to resolve its Holding.</param>
    public RegisterAssetCommandHandler(
        IAssetRepository assetRepository,
        IAssetModelRepository assetModelRepository,
        IConfigurationLookupService configurationLookupService,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _assetRepository = assetRepository;
        _assetModelRepository = assetModelRepository;
        _configurationLookupService = configurationLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the registration use case.</summary>
    /// <param name="request">The registration command, containing the target Organization, Asset Model, Color, identification code, and optional identity fields.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Result{Guid}"/> containing the new Asset's identifier on success; otherwise a
    /// validation, not-found, conflict, or authorization error.
    /// </returns>
    public async Task<Result<Guid>> Handle(RegisterAssetCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var organizationExists = await _organizationLookupService.ExistsAsync(request.OrganizationId, cancellationToken);

        if (!organizationExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.OrganizationNotFound(request.OrganizationId));
        }

        var assetModelId = global::Asset.Domain.AssetModelId.From(request.AssetModelId);
        var assetModel = await _assetModelRepository.GetByIdAsync(assetModelId, cancellationToken);

        if (assetModel is null)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.AssetModelNotFound(request.AssetModelId));
        }

        if (holdingId is null || assetModel.HoldingId != holdingId.Value)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.AssetModelHoldingMismatch());
        }

        // Color now lives in the Configuration module and is
        // Holding-scoped — existence + Holding-match are both checked
        // via this single cross-module lookup call, reusing the same
        // holdingId already resolved above for the AssetModel check
        // (chat, 2026-08-30). Unlike AssetModel, there is no local
        // Color entity to load here, so no further per-field
        // comparison is needed or possible after this check.
        var colorExists = await _configurationLookupService.ColorExistsInHoldingAsync(
            request.ColorId, holdingId.Value, cancellationToken);

        if (!colorExists)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.ColorNotFoundInHolding(request.ColorId));
        }

        var codeAlreadyUsed = await _assetRepository.ExistsWithCodeAsync(
            request.OrganizationId,
            request.Code,
            cancellationToken);

        if (codeAlreadyUsed)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.DuplicateCode(request.Code));
        }

        var result = global::Asset.Domain.Asset.Register(
            request.OrganizationId,
            request.Code,
            request.Name,
            assetModelId,
            request.ColorId,
            request.SerialNumber,
            request.ChassisNumber,
            request.BodyNumber,
            request.Vin,
            request.LicensePlate,
            request.ManufactureYear,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _assetRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
