using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain.Events;

/// <summary>Raised when an existing <see cref="LubricantOverflowReport"/> is edited (header, lines, or personnel entries).</summary>
public sealed record LubricantOverflowReportUpdated(LubricantOverflowReportId LubricantOverflowReportId, DateTimeOffset OccurredOn) : IDomainEvent;