namespace MachineryManagerEnterprise.Consumption.Presentation.Contracts;

/// <summary>Request body for editing an existing fuel-consumption record. The Id is supplied via the route.</summary>
/// <param name="FuelTypeId">The specific Configuration FuelType selected by the user (editable).</param>
/// <param name="Quantity">The fuel quantity, in the record's fixed unit.</param>
/// <param name="MeterReading">The Asset's meter reading at the moment of fueling.</param>
/// <param name="DeliveredByPersonnelId">The Personnel who delivered the fuel.</param>
/// <param name="ReceivedByPersonnelId">The Personnel who received the fuel.</param>
/// <param name="RecordedAtUtc">The date and time the fueling actually took place.</param>
/// <param name="Notes">Optional free-form notes.</param>
public sealed record EditFuelConsumptionRequest(
    Guid FuelTypeId,
    decimal Quantity,
    decimal MeterReading,
    Guid DeliveredByPersonnelId,
    Guid ReceivedByPersonnelId,
    DateTimeOffset RecordedAtUtc,
    string? Notes);
