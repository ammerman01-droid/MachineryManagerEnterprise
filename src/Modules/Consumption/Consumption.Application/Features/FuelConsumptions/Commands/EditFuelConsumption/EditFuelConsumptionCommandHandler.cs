using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.EditFuelConsumption;

/// <summary>Handles <see cref="EditFuelConsumptionCommand"/>.</summary>
public sealed class EditFuelConsumptionCommandHandler : IRequestHandler<EditFuelConsumptionCommand, Result>
{
    private const string RequiredPermission = "FuelConsumption.Edit";

    private readonly IFuelConsumptionRepository _fuelConsumptionRepository;
    private readonly IConsumptionUnitOfWork _unitOfWork;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly IConfigurationLookupService _configurationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>Initializes a new instance of the <see cref="EditFuelConsumptionCommandHandler"/> class.</summary>
    public EditFuelConsumptionCommandHandler(
        IFuelConsumptionRepository fuelConsumptionRepository,
        IConsumptionUnitOfWork unitOfWork,
        IOrganizationLookupService organizationLookupService,
        IPersonnelLookupService personnelLookupService,
        IConfigurationLookupService configurationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IDateTimeProvider dateTimeProvider)
    {
        _fuelConsumptionRepository = fuelConsumptionRepository;
        _unitOfWork = unitOfWork;
        _organizationLookupService = organizationLookupService;
        _personnelLookupService = personnelLookupService;
        _configurationLookupService = configurationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(EditFuelConsumptionCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(FuelConsumptionErrors.NotAuthorized());
        }

        var fuelConsumptionId = FuelConsumptionId.From(request.Id);
        var record = await _fuelConsumptionRepository.GetByIdAsync(fuelConsumptionId, cancellationToken);

        if (record is null)
        {
            return Result.Failure(FuelConsumptionErrors.NotFound(request.Id));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(record.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, record.OrganizationId, record.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(FuelConsumptionErrors.NotAuthorized());
        }

        var personnelValidation = await ValidatePersonnelAsync(
            request.DeliveredByPersonnelId, request.ReceivedByPersonnelId, record.OrganizationId, cancellationToken);

        if (personnelValidation.IsFailure)
        {
            return personnelValidation;
        }

        // Neighbor-only monotonic check (chat, 2026-09-15), excluding this record from its own neighbor search.
        var chainValidation = await ValidateMeterReadingChainAsync(
            record.AssetId, request.MeterReading, request.RecordedAtUtc, record.Id.Value, cancellationToken);

        if (chainValidation.IsFailure)
        {
            return chainValidation;
        }

        // FuelType is now editable (chat, 2026-09-22 — supersedes the 2026-09-16 assumption
        // that price is never re-derived on edit): re-resolve and re-validate exactly as on
        // Record, then re-snapshot the price from the (possibly new) FuelType.
        var fuelType = await _configurationLookupService.GetFuelTypeAsync(request.FuelTypeId, cancellationToken);

        if (fuelType is null)
        {
            return Result.Failure(FuelConsumptionErrors.FuelTypeNotFound(request.FuelTypeId));
        }

        if (holdingId is null || fuelType.HoldingId != holdingId.Value)
        {
            return Result.Failure(FuelConsumptionErrors.FuelTypeHoldingMismatch());
        }

        if (fuelType.Kind != record.FuelKind)
        {
            return Result.Failure(FuelConsumptionErrors.FuelTypeKindMismatch(record.FuelKind, fuelType.Kind));
        }

        var editResult = record.Edit(
            request.FuelTypeId,
            fuelType.Price,
            request.Quantity,
            request.MeterReading,
            request.DeliveredByPersonnelId,
            request.ReceivedByPersonnelId,
            request.RecordedAtUtc,
            request.Notes,
            _dateTimeProvider);

        if (editResult.IsFailure)
        {
            return editResult;
        }

        _fuelConsumptionRepository.Update(record);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
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
        Guid excludingId,
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
