using MachineryManagerEnterprise.Consumption.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.RecordFuelConsumption;

/// <summary>Records a new fuel-fill event for one fuel slot of an Asset.</summary>
public sealed record RecordFuelConsumptionCommand(
    Guid AssetId,
    FuelSlot FuelSlot,
    Guid FuelTypeId,
    decimal Quantity,
    decimal MeterReading,
    Guid DeliveredByPersonnelId,
    Guid ReceivedByPersonnelId,
    DateTimeOffset RecordedAtUtc,
    string? Notes) : IRequest<Result<Guid>>;
