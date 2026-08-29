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
    private readonly IColorRepository _colorRepository;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterAssetCommandHandler"/> class.</summary>
    public RegisterAssetCommandHandler(
        IAssetRepository assetRepository,
        IAssetModelRepository assetModelRepository,
        IColorRepository colorRepository,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _assetRepository = assetRepository;
        _assetModelRepository = assetModelRepository;
        _colorRepository = colorRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the registration use case.</summary>
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

        var colorId = global::Asset.Domain.ColorId.From(request.ColorId);
        var color = await _colorRepository.GetByIdAsync(colorId, cancellationToken);

        if (color is null)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.ColorNotFound(request.ColorId));
        }

        if (color.OrganizationId != request.OrganizationId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.AssetErrors.ColorOrganizationMismatch());
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
            colorId,
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