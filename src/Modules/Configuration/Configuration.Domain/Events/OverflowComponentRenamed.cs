using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when an <see cref="OverflowComponent"/> is renamed.</summary>
public sealed record OverflowComponentRenamed(OverflowComponentId OverflowComponentId, string Name, DateTimeOffset OccurredOn) : IDomainEvent;