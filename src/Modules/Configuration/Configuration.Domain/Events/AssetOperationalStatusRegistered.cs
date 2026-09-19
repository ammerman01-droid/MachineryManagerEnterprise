using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Domain event raised when a new AssetOperationalStatus is registered.</summary>
public sealed record AssetOperationalStatusRegistered(
    AssetOperationalStatusId Id,
    Guid HoldingId,
    string Name,
    DateTimeOffset OccurredOn) : IDomainEvent;