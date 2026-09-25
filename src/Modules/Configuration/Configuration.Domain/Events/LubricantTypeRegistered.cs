using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Raised when a new <see cref="LubricantType"/> is registered.</summary>
public sealed record LubricantTypeRegistered(LubricantTypeId LubricantTypeId, Guid HoldingId, string Name, DateTimeOffset OccurredOn) : IDomainEvent;