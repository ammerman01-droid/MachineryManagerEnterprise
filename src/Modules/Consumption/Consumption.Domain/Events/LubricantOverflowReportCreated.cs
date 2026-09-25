using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain.Events;

/// <summary>Raised when a new <see cref="LubricantOverflowReport"/> is created.</summary>
public sealed record LubricantOverflowReportCreated(
    LubricantOverflowReportId LubricantOverflowReportId,
    Guid OrganizationId,
    Guid ProjectId,
    Guid AssetId,
    DateOnly ReportDate,
    DateTimeOffset OccurredOn) : IDomainEvent;