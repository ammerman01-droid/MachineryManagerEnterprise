using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.EditFuelConsumption;

/// <summary>
/// Edits the factual details of an existing fuel-consumption record,
/// including which FuelType priced it (chat, 2026-09-22 — FuelType is
/// now editable). Structural/snapshotted fields (Asset, fuel slot,
/// fuel kind/unit, meter unit) remain not editable — see
/// <see cref="MachineryManagerEnterprise.Consumption.Domain.FuelConsumption.Edit"/>.
/// </summary>
public sealed record EditFuelConsumptionCommand(
    Guid Id,
    Guid FuelTypeId,
    decimal Quantity,
    decimal MeterReading,
    Guid DeliveredByPersonnelId,
    Guid ReceivedByPersonnelId,
    DateTimeOffset RecordedAtUtc,
    string? Notes) : IRequest<Result>;
