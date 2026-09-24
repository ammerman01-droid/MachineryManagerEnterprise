using MachineryManagerEnterprise.Consumption.Domain;
using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Consumption.Presentation.Contracts;

/// <summary>Request body for recording a new fuel-consumption event.</summary>
/// <param name="AssetId">The Asset that was fueled.</param>
/// <param name="FuelSlot">Which of the Asset's fuel slots (Primary/Secondary) was filled.</param>
/// <param name="FuelTypeId">The specific Configuration FuelType selected by the user.</param>
/// <param name="Quantity">The fuel quantity, in the Asset's configured unit for this slot.</param>
/// <param name="MeterReading">The Asset's meter reading (odometer/hour-meter) at the moment of fueling.</param>
/// <param name="DeliveredByPersonnelId">The Personnel who delivered the fuel.</param>
/// <param name="ReceivedByPersonnelId">The Personnel who received the fuel.</param>
/// <param name="RecordedAtUtc">The date and time the fueling actually took place.</param>
/// <param name="Notes">Optional free-form notes.</param>
public sealed record RecordFuelConsumptionRequest(
    Guid AssetId,
    FuelSlot FuelSlot,
    Guid FuelTypeId,
    decimal Quantity,
    decimal MeterReading,
    Guid DeliveredByPersonnelId,
    Guid ReceivedByPersonnelId,
    DateTimeOffset RecordedAtUtc,
    string? Notes);
