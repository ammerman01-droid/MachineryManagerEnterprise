using MachineryManagerEnterprise.Asset.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.UpdateAsset;

/// <summary>Handles <see cref="UpdateAssetCommand"/>, re-running the same cross-module checks as registration.</summary>
public sealed class UpdateAssetCommandHandler : IRequestHandler<UpdateAssetCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IAssetRepository _assetRepository;
    private readonly IAssetModelRepository _assetModelRepository;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly IProjectLookupService _projectLookupService;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="UpdateAssetCommandHandler"/> class.</summary>
    public UpdateAssetCommandHandler(
        IAssetRepository assetRepository,
        IAssetModelRepository assetModelRepository,
        IConfigurationLookupService configurationLookupService,
        IProjectLookupService projectLookupService,
        IAssetUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _assetRepository = assetRepository;
        _assetModelRepository = assetModelRepository;
        _configurationLookupService = configurationLookupService;
        _projectLookupService = projectLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the update use case.</summary>
    public async Task<Result> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var asset = await _assetRepository.GetByIdAsync(
            global::Asset.Domain.AssetId.From(request.AssetId), cancellationToken);

        if (asset is null)
        {
            return Result.Failure(Error.NotFound("Asset.NotFound", $"Asset with id {request.AssetId} was not found."));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(asset.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, asset.OrganizationId, request.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var assetModelId = global::Asset.Domain.AssetModelId.From(request.AssetModelId);
        var assetModel = await _assetModelRepository.GetByIdAsync(assetModelId, cancellationToken);

        if (assetModel is null)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.AssetModelNotFound(request.AssetModelId));
        }

        if (holdingId is null || assetModel.HoldingId != holdingId.Value)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.AssetModelHoldingMismatch());
        }

        var colorExists = await _configurationLookupService.ColorExistsInHoldingAsync(
            request.ColorId, holdingId.Value, cancellationToken);

        if (!colorExists)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.ColorNotFoundInHolding(request.ColorId));
        }

        var projectExists = await _projectLookupService.ExistsAsync(request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.ProjectNotFound(request.ProjectId));
        }

        var projectOrganizationId = await _projectLookupService.GetOrganizationIdAsync(request.ProjectId, cancellationToken);

        if (projectOrganizationId != asset.OrganizationId)
        {
            return Result.Failure(global::Asset.Domain.AssetErrors.ProjectOrganizationMismatch());
        }

        // The identification Code is editable (chat, 2026-09-19); when it
        // actually changes it must stay unique within the Organization.
        var newCode = request.Code.Trim();

        if (!string.Equals(newCode, asset.Code, StringComparison.Ordinal))
        {
            var codeInUse = await _assetRepository.ExistsOtherWithCodeAsync(
                asset.OrganizationId, newCode, asset.Id, cancellationToken);

            if (codeInUse)
            {
                return Result.Failure(global::Asset.Domain.AssetErrors.DuplicateCode(newCode));
            }
        }

        var result = asset.UpdateDetails(
            request.Code,
            request.Name,
            assetModelId,
            request.ColorId,
            request.ProjectId,
            request.SerialNumber,
            request.ChassisNumber,
            request.BodyNumber,
            request.Vin,
            request.LicensePlate,
            request.ManufactureYear,
            request.MeterReadingUnit,
            request.PrimaryFuelKind,
            request.PrimaryFuelUnit,
            request.SecondaryFuelKind,
            request.SecondaryFuelUnit);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
