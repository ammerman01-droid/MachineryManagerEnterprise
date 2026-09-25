using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain.Events;

/// <summary>Raised when a <see cref="LubricantOverflowReport"/> is deleted.</summary>
public sealed record LubricantOverflowReportDeleted(LubricantOverflowReportId LubricantOverflowReportId, Guid OrganizationId, DateTimeOffset OccurredOn) : IDomainEvent;