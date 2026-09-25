using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.DeleteLubricantOverflowReport;

/// <summary>Deletes a Lubricant Overflow Report, provided its date has not been frozen by an Organization Administrator.</summary>
public sealed record DeleteLubricantOverflowReportCommand(Guid LubricantOverflowReportId) : IRequest<Result>;
