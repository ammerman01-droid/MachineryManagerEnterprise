using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Consumption.Domain.Events;

/// <summary>Raised when an existing fuel-consumption record is edited in place.</summary>
public sealed record FuelConsumptionUpdated(
    FuelConsumptionId FuelConsumptionId,
    Guid AssetId,
    DateTimeOffset OccurredOn) : IDomainEvent;
