using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Consumption.Domain.Events;

/// <summary>Raised when a new fuel-consumption record is created for an Asset.</summary>
public sealed record FuelConsumptionRecorded(
    FuelConsumptionId FuelConsumptionId,
    Guid AssetId,
    DateTimeOffset OccurredOn) : IDomainEvent;
