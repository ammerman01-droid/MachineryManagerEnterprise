using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain.Events;

/// <summary>Raised when an Organization's Consumption freeze threshold is set or changed.</summary>
public sealed record ConsumptionFreezeThresholdSet(Guid OrganizationId, DateOnly ThresholdDate, DateTimeOffset OccurredOn) : IDomainEvent;