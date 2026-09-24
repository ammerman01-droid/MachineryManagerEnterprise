using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.RecordFuelConsumption;

/// <summary>Handles <see cref="RecordFuelConsumptionCommand"/>.</summary>
public sealed class RecordFuelConsumptionCommandHandler
    : IRequestHandler<RecordFuelConsumptionCommand, Result<Guid>>
{
    private const string RequiredPermission = "FuelConsumption.Create";

    private readonly IFuelConsumptionRepository _fuelConsumptionRepository;
    private readonly IConsumptionUnitOfWork _unitOfWork;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>Initializes a new instance of the <see cref="RecordFuelConsumptionCommandHandler"/> class.</summary>
    public RecordFuelConsumptionCommandHandler(
        IFuelConsumptionRepository fuelConsumptionRepository,
        IConsumptionUnitOfWork unitOfWork,
        IAssetLookupService assetLookupService,
        IOrganizationLookupService organizationLookupService,
        IPersonnelLookupService personnelLookupService,
        IConfigurationLookupService configurationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IDateTimeProvider dateTimeProvider)
    {
        _fuelConsumptionRepository = fuelConsumptionRepository;
        _unitOfWork = unitOfWork;
        _assetLookupService = assetLookupService;
        _organizationLookupService = organizationLookupService;
        _personnelLookupService = personnelLookupService;
        _configurationLookupService = configurationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(RecordFuelConsumptionCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(FuelConsumptionErrors.NotAuthorized());
        }

        // 1) Resolve the Asset's consumption-relevant fields (cross-module lookup).
        var asset = await _assetLookupService.GetConsumptionSnapshotAsync(request.AssetId, cancellationToken);

        if (asset is null)
        {
            return Result.Failure<Guid>(FuelConsumptionErrors.AssetNotFound(request.AssetId));
        }

        // 2) Authorization — scoped to the Asset's Holding/Organization/Project.
        var holdingId = await _organizationLookupService.GetHoldingIdAsync(asset.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, asset.OrganizationId, asset.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(FuelConsumptionErrors.NotAuthorized());
        }

        // 3) The Asset must have a meter reading unit configured.
        if (asset.MeterReadingUnit is not { } meterReadingUnit)
        {
            return Result.Failure<Guid>(FuelConsumptionErrors.MeterUnitNotConfiguredOnAsset());
        }

        // 4) Resolve which fuel kind/unit applies to the requested slot.
        var fuelResult = ResolveFuel(asset, request.FuelSlot);

        if (fuelResult.IsFailure)
        {
            return Result.Failure<Guid>(fuelResult.Error);
        }

        var (fuelKind, fuelUnit) = fuelResult.Value;

        // 5) Resolve the specific FuelType Aggregate the user selected (chat, 2026-09-22 —
        //    FuelKind is only a filter dimension now; price always comes from the exact
        //    FuelType chosen, since a Holding may register several FuelTypes sharing FuelKind).
        var fuelType = await _configurationLookupService.GetFuelTypeAsync(request.FuelTypeId, cancellationToken);

        if (fuelType is null)
        {
            return Result.Failure<Guid>(FuelConsumptionErrors.FuelTypeNotFound(request.FuelTypeId));
        }

        if (holdingId is null || fuelType.HoldingId != holdingId.Value)
        {
            return Result.Failure<Guid>(FuelConsumptionErrors.FuelTypeHoldingMismatch());
        }

        if (fuelType.Kind != fuelKind)
        {
            return Result.Failure<Guid>(FuelConsumptionErrors.FuelTypeKindMismatch(fuelKind, fuelType.Kind));
        }

        var unitPrice = fuelType.Price;

        // 6) Validate the delivering/receiving Personnel (cross-module lookup).
        var personnelValidation = await ValidatePersonnelAsync(
            request.DeliveredByPersonnelId, request.ReceivedByPersonnelId, asset.OrganizationId, cancellationToken);

        if (personnelValidation.IsFailure)
        {
            return Result.Failure<Guid>(personnelValidation.Error);
        }

        // 7) Enforce the monotonic meter-reading chain against this Asset's immediate neighbors only
        //    (chat, 2026-09-15 — no full chain re-validation).
        var chainValidation = await ValidateMeterReadingChainAsync(
            request.AssetId, request.MeterReading, request.RecordedAtUtc, excludingId: null, cancellationToken);

        if (chainValidation.IsFailure)
        {
            return Result.Failure<Guid>(chainValidation.Error);
        }

        // 8) Create the aggregate.
        var fuelConsumptionResult = FuelConsumption.Record(
            request.AssetId,
            asset.OrganizationId,
            asset.ProjectId,
            request.FuelSlot,
            fuelKind,
            fuelUnit,
            request.FuelTypeId,
            unitPrice,
            request.Quantity,
            meterReadingUnit,
            request.MeterReading,
            request.DeliveredByPersonnelId,
            request.ReceivedByPersonnelId,
            request.RecordedAtUtc,
            request.Notes,
            _dateTimeProvider);

        if (fuelConsumptionResult.IsFailure)
        {
            return Result.Failure<Guid>(fuelConsumptionResult.Error);
        }

        _fuelConsumptionRepository.Add(fuelConsumptionResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(fuelConsumptionResult.Value.Id.Value);
    }

    /// <summary>
    /// Determines the (FuelKind, FuelUnit) pair for a requested slot
    /// from the Asset's snapshot, failing if that slot is not
    /// configured on the Asset.
    /// </summary>
    internal static Result<(FuelKind FuelKind, FuelUnit FuelUnit)> ResolveFuel(
        AssetConsumptionSnapshot asset, FuelSlot fuelSlot)
    {
        var (fuelKind, fuelUnit) = fuelSlot switch
        {
            FuelSlot.Primary => (asset.PrimaryFuelKind, asset.PrimaryFuelUnit),
            FuelSlot.Secondary => (asset.SecondaryFuelKind, asset.SecondaryFuelUnit),
            _ => (null, null)
        };

        return fuelKind is null || fuelUnit is null
            ? Result.Failure<(FuelKind, FuelUnit)>(FuelConsumptionErrors.FuelSlotNotConfiguredOnAsset(fuelSlot))
            : Result.Success((fuelKind.Value, fuelUnit.Value));
    }

    private async Task<Result> ValidatePersonnelAsync(
        Guid deliveredByPersonnelId,
        Guid receivedByPersonnelId,
        Guid organizationId,
        CancellationToken cancellationToken)
    {
        var deliveredByOrganizationId = await _personnelLookupService.GetOrganizationIdAsync(deliveredByPersonnelId, cancellationToken);

        if (deliveredByOrganizationId is null)
        {
            return Result.Failure(FuelConsumptionErrors.DeliveredByPersonnelNotFound(deliveredByPersonnelId));
        }

        if (deliveredByOrganizationId != organizationId)
        {
            return Result.Failure(FuelConsumptionErrors.PersonnelOrganizationMismatch());
        }

        var receivedByOrganizationId = await _personnelLookupService.GetOrganizationIdAsync(receivedByPersonnelId, cancellationToken);

        if (receivedByOrganizationId is null)
        {
            return Result.Failure(FuelConsumptionErrors.ReceivedByPersonnelNotFound(receivedByPersonnelId));
        }

        if (receivedByOrganizationId != organizationId)
        {
            return Result.Failure(FuelConsumptionErrors.PersonnelOrganizationMismatch());
        }

        return Result.Success();
    }

    private async Task<Result> ValidateMeterReadingChainAsync(
        Guid assetId,
        decimal meterReading,
        DateTimeOffset recordedAtUtc,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        var previous = await _fuelConsumptionRepository.GetPreviousAsync(assetId, recordedAtUtc, excludingId, cancellationToken);

        if (previous is not null && meterReading < previous.MeterReading)
        {
            return Result.Failure(FuelConsumptionErrors.MeterReadingBelowPrevious(previous.MeterReading));
        }

        var next = await _fuelConsumptionRepository.GetNextAsync(assetId, recordedAtUtc, excludingId, cancellationToken);

        if (next is not null && meterReading > next.MeterReading)
        {
            return Result.Failure(FuelConsumptionErrors.MeterReadingAboveNext(next.MeterReading));
        }

        return Result.Success();
    }
}
