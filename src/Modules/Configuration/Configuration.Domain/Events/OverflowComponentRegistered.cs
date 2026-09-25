using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a new <see cref="OverflowComponent"/> is registered.</summary>
public sealed record OverflowComponentRegistered(OverflowComponentId OverflowComponentId, Guid HoldingId, string Name, DateTimeOffset OccurredOn) : IDomainEvent;