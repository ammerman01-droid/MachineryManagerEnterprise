namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Dtos;

/// <summary>Read model for a single fuel-consumption record.</summary>
public sealed record FuelConsumptionDto(
    Guid Id,
    Guid AssetId,
    Guid OrganizationId,
    Guid ProjectId,
    string FuelSlot,
    string FuelKind,
    string FuelUnit,
    Guid FuelTypeId,
    decimal UnitPriceSnapshot,
    decimal Quantity,
    string MeterReadingUnit,
    decimal MeterReading,
    Guid DeliveredByPersonnelId,
    Guid ReceivedByPersonnelId,
    DateTimeOffset RecordedAtUtc,
    string? Notes);
