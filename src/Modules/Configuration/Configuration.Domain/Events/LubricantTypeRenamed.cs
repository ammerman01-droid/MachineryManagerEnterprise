using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a <see cref="LubricantType"/> is renamed.</summary>
public sealed record LubricantTypeRenamed(LubricantTypeId LubricantTypeId, string Name, DateTimeOffset OccurredOn) : IDomainEvent;